# Scanner

一个基于 **.NET Framework 4.5.2 + WinForms** 的 Windows 桌面扫描程序，通过 **TWAIN 1.9** 协议直接调用扫描仪驱动，支持枚举数据源、自动进纸器（ADF）双面连续扫描，并把每页图像落盘后在窗体内预览与翻页。

> 本项目是**纯本地桌面应用**：没有服务端、没有网络接口、没有数据库，也不需要任何云资源。唯一的第三方库是日志组件 `log4net 2.0.17`。

---

## 目录

- [1. 项目概览](#1-项目概览)
- [2. 解决方案与工程结构](#2-解决方案与工程结构)
- [3. 架构与核心模块](#3-架构与核心模块)
- [4. 一次扫描的完整调用链](#4-一次扫描的完整调用链)
- [5. 配置、参数与产物](#5-配置参数与产物)
- [6. 构建与环境要求](#6-构建与环境要求)
- [7. 运行前置条件与使用说明](#7-运行前置条件与使用说明)
- [8. 已知限制与注意事项](#8-已知限制与注意事项)
- [9. 依赖清单](#9-依赖清单)
- [10. 常见问题排查](#10-常见问题排查)
- [11. 许可证](#11-许可证)

---

## 1. 项目概览

| 项目 | 内容 |
| --- | --- |
| 应用类型 | Windows 窗体应用（`WinExe`），单窗体 `FormScan` |
| 目标框架 | .NET Framework **4.5.2**（`supportedRuntime v4.0 / sku .NETFramework,Version=v4.5.2`） |
| 工程格式 | 旧式非 SDK `.csproj`（`ToolsVersion="15.0"`），解决方案 `Format Version 12.00 / VS17` |
| 主要语言 | C#（WinForms / `System.Drawing` / P/Invoke） |
| 扫描协议 | TWAIN **1.9**，通过 `twain_32.dll` 的 `DSM_Entry` 序号导出 `#1` 调用 |
| 图像传输 | Native DIB（内存位图句柄），经 `BitmapRenderer` 转成 GDI+ `Bitmap` |
| 图像输出 | 每页一个独立文件，GUID 命名，扩展名 `.tiff`，存于 `<程序目录>\image` |
| 日志 | `log4net 2.0.17`，控制台 + 按日期滚动文件（`logs/scanner.log`） |
| 配置方式 | 无 `appSettings`、无命令行参数；扫描参数在 `Scanner/FormScan.cs` 中硬编码 |
| 程序集版本 | `1.0.0.0`（4 个工程一致），版权标记 `Copyright ©  2024` |
| 测试 / CI | 无单元测试工程、无 CI 脚本、无安装/发布脚本 |
| 许可证 | MIT，见 [`LICENSE`](LICENSE) |

**它做什么**：启动 → 枚举本机 TWAIN 数据源并填入下拉框 → 用户选扫描仪点「扫描」→ 以 ADF + 双面 + 300dpi 彩色参数启动扫描 → 驱动每传回一页就保存一张图片 → 扫描结束后在「微缩视图 / 垂直视图」中分页预览。

**它不做什么**：不生成 PDF、不合成多页 TIFF、不提供图像编辑/OCR、不上传或同步、不提供配置界面、不需要登录。

---

## 2. 解决方案与工程结构

`Scanner.sln` 包含 4 个工程，只有 `Debug|Any CPU` 与 `Release|Any CPU` 两种配置：

| 解决方案中的工程名 | 目录 / csproj | 类型 | 程序集名 | 根命名空间 | 目标框架 |
| --- | --- | --- | --- | --- | --- |
| `Scanner` | `Scanner/Scanner.csproj` | WinExe | `Scanner` | `Scanner` | v4.5.2 |
| `Scanner.Twain32` | `Scanner.Twain/Scanner.Twain32.csproj` | Library | **`Scanner.Twain`** | `Scanner.Twain` | v4.5.2 |
| `Scanner.Toolkit` | `Scanner.Toolkit/Scanner.Toolkit.csproj` | Library | `Scanner.Toolkit` | `Scanner.Toolkit` | v4.5.2 |
| `Scanner.Visual` | `Scanner.Visual/Scanner.Visual.csproj` | Library | `Scanner.Visual` | `Scanner.Visual` | v4.5.2 |

> **命名不一致提示**：解决方案里叫 `Scanner.Twain32`，csproj 文件名也是 `Scanner.Twain32.csproj`，但目录是 `Scanner.Twain/`，编译产物和命名空间却是 `Scanner.Twain`。检索代码时请以 `Scanner.Twain` 命名空间为准。

**依赖关系**（单向星型，只有主工程引用其余 3 个库，库之间互不引用）：

```text
Scanner (WinExe, 主程序)
 ├── Scanner.Toolkit   —— 路径/文件/系统工具
 ├── Scanner.Twain32  —— TWAIN 协议封装（程序集 Scanner.Twain.dll）
 └── Scanner.Visual   —— 分页 UserControl
```

全部系统引用均为 .NET Framework BCL（`System`、`System.Drawing`、`System.Windows.Forms`、`System.Net.Http`、`System.Web`、`System.Xml*` 等）。仅 `Scanner` 与 `Scanner.Twain32` 通过 `HintPath` 引用 `..\packages\log4net.2.0.17\lib\net45\log4net.dll`。4 个工程均无强名称签名、无 `AllowUnsafeBlocks`、无 `StartupObject`、无应用图标。

### 目录树（源码部分）

```text
Scanner/
├── Scanner.sln                 解决方案
├── .gitignore / .gitattributes
├── LICENSE                    MIT 许可证
├── README.md                  本文档
├── packages/                   NuGet 还原产物（log4net.2.0.17）
├── Scanner/                    主程序（8 个 .cs）
│   ├── Program.cs              [STAThread] Main 入口
│   ├── FormScan.cs             主窗体：扫描调度、图片落盘、预览分页、控件等比缩放
│   ├── FormScan.Designer.cs    WinForms 设计器代码（中文 UI 文本硬编码于此）
│   ├── WinFormsWindowMessageHook.cs   IMessageFilter ↔ TWAIN 消息钩子桥接
│   ├── App.config              log4net + supportedRuntime
│   ├── packages.config         log4net 2.0.17 / net452
│   └── Properties/             AssemblyInfo / Resources / Settings（后两者为空模板）
├── Scanner.Twain/              TWAIN 协议封装（65 个 .cs，核心）
│   ├── *.cs                    门面、会话、能力协商、设置、事件参数、异常（22 个）
│   ├── Twain32Native/          TWAIN 结构体、枚举、常量与 [DllImport]（37 个）
│   ├── Win32Native/            Kernel32/Gdi32/User32 P/Invoke、DIB→Bitmap（6 个）
│   ├── App.config / packages.config
│   └── Properties/AssemblyInfo.cs
├── Scanner.Toolkit/            通用工具类（6 个 .cs）
│   ├── FilePath.cs             路径拼接（自动建目录）、文件操作、图片格式/扩展名互转
│   ├── OSHelper.cs             进程、注册表、ACL、TCP/ICMP Ping、端口检测
│   ├── ScriptUtil.cs           FIPS 开关、启动/结束进程、`lisscanner` URL 协议注册
│   ├── StringHelper.cs / Naming.cs
│   └── Properties/AssemblyInfo.cs
└── Scanner.Visual/             WinForms 用户控件（3 个 .cs）
    └── PageControl.cs          带 `<< < > >>` 按钮与「第 N/M 页」标签的分页控件
```

---

## 3. 架构与核心模块

### 3.1 `Scanner.Twain` —— TWAIN 1.9 协议封装（项目核心，65 个源文件）

按层次划分为 4 层：

| 层 | 关键类型 | 职责 |
| --- | --- | --- |
| **门面层** | `TwainControl` | 对外唯一入口：`GetSourceNames` / `GetDefaultSourceName` / `SelectSource` / `StartScanning`；只暴露 `TransferImage`、`ScanningComplete` 两个事件（其余事件在源码中被注释保留） |
| **会话层** | `DataSourceManager`、`DataSource`、`Capability`、`CapabilityResult`、`ScanSettings` 及 `Area/Page/Resolution/RotationSettings`、`IWindowsMessageHook`、7 个 `EventArgs`、`TwainException` / `FeederEmptyException`、`Diagnostics` | DSM 生命周期、数据源枚举、能力协商、WinForms 消息循环驱动、多页传输循环 |
| **TWAIN Native 层** | `Twain32Native`（11 个 `DllImport("twain_32.dll", EntryPoint="#1")`）、`Identity`、`TwainVersion`、`UserInterface`、`Event`、`ImageInfo`、`ImageLayout`、`Frame`、`Fix32`、`Status`、`PendingXfers`、`TwainCapability` / `CapabilityOneValue` / `EnumValue` / `ArrayValue` | TWAIN 数据结构（`[StructLayout(LayoutKind.Sequential, Pack=2)]`）与常量全表：`Capabilities`、`TWMessage`、`DataArgumentType`、`DataGroup`、`TwainResult`、`ConditionCode`、`TwainType`、`ContainerType`、`PixelType`、`PageType`、`ImageFileFormat`、`Units`、`Country`、`Language` 等；`TwainConstants` 声明协议版本 1.9 |
| **Win32 层** | `Kernel32Native`（`GlobalAlloc/Lock/Unlock/Free`）、`Gdi32Native`（`SetDIBitsToDevice`）、`User32Native`（`GetMessagePos/Time`）、`BitmapInfoHeader`、`WindowsMessage`、`BitmapRenderer` | 把 TWAIN 返回的全局内存 DIB 句柄渲染成 GDI+ `Bitmap`，并换算 DPI（`PpmToDpi`） |

要点：

- `Twain32Native` 中的 11 个 P/Invoke 全部映射到同一个 `DSM_Entry` 序号导出，靠 `DataGroup / DataArgumentType / TWMessage` 三元组区分操作（`OpenDSM`、`OpenDS`、`EnableDS`、`ProcessEvent`、`ImageNativeXfer`、`EndXfer`、`Reset`、`Get/Set Capability` 等）。
- `DataSource.Open(ScanSettings)` 会按固定顺序协商十余项能力：进度指示、传输页数、ADF、双面、纸张尺寸、方向、扫描区域、色彩、分辨率、旋转、边框检测。不支持某项能力时代码用空 `catch { }` 跳过（驱动兼容性策略），因此不同厂商扫描仪的实际效果会有差异。
- `TwainCapability` 目前只实现 `CapabilityOneValue` 容器（`GlobalAlloc/GlobalLock/Marshal.StructureToPtr`），`EnumValue` / `ArrayValue` 结构体虽已定义但协商流程未使用。
- 传输机制**固定为 Native DIB**（`DAT_ImageNativeXfer`）。`TransferMechanism` 与 `ImageFileFormat` 枚举仅是 TWAIN 常量定义，流程中从未设置，因此没有 File/Memory 传输，也没有 TWAIN 侧的文件格式协商。
- `BitmapRenderer` 内含 `Debug.Assert(Marshal.SizeOf(typeof(IntPtr)) == 4)` 与 `ToInt32()`，源码注释明确写着 `// The following code only works on x86` —— 见 [§6 平台要求](#6-构建与环境要求)。

### 3.2 `Scanner` —— 主程序（WinForms）

- `Program.Main()`：`[STAThread]`，仅 `EnableVisualStyles` → `SetCompatibleTextRenderingDefault(false)` → `Application.Run(new FormScan())`。**无参数、无命令行解析**。
- `FormScan` 构造：初始化分页控件 → 注册窗体 `Resize` 实现控件等比缩放（`SetTag/SetControls/ReWinformLayout`）→ `LoadScanner()` 创建 `TwainControl` 并枚举数据源绑定下拉框 → **清空 `<程序目录>\image` 下的全部文件**。
- `LoadScanner()` 订阅两个事件：`TransferImage` 把 `args.Image` 以 `Guid.NewGuid() + ".tiff"` 保存到 `image` 目录；`ScanningComplete` 恢复窗体、切回第一个预览 Tab 并重新加载图片列表。
- 扫描按钮 `bntScan_Click`：取下拉框数据源名 → `SelectSource` → 构造 `ScanSettings` → `StartScanning`；捕获 `TwainException` 后仅 `MessageBox.Show(ex.Message)`。
- 预览：`Image.FromFile` → `ImageList`/`ListView`（微缩视图，分页）与 `PictureBox.Load`（垂直视图）。设计器中还有第 3 个「水平视图」Tab，但事件 `switch` 只处理 case 0/1。
- `WinFormsWindowMessageHook`：实现 `IWindowsMessageHook` 与 `IMessageFilter`，通过 `Application.AddMessageFilter` 把 Win32 消息转发给 TWAIN DSM，这是无 UI 扫描（`ShowTwainUI=false`）能收到 `XFerReady` 的关键。

### 3.3 `Scanner.Toolkit` —— 通用工具

`FilePath`（约 656 行）提供应用/日志/数据/临时/更新等路径获取、`PathCombine`（去掉开头反斜杠、**自动创建目录**、返回值以 `\` 结尾）、文件增删改查与 ACL、`GetPictureList`（`.jpg/.bmp/.png/.tif/.gif/.tiff`）、`GetImageFormatExtension` / `GetImageFormat` / `IsImageFormat`（Bmp/Emf/Exif/Gif/Icon/Jpeg/Png/Tiff/Wmf 互转）。`OSHelper` 提供进程、注册表、`RegSvr/RegAsm`、Everyone/Users 授权、TCP/ICMP `Ping` 与端口占用检测。`ScriptUtil` 提供 FIPS 开关、进程控制与 `lisscanner://` URL 协议注册（指向 `Scanner.exe`）。

> 主程序实际只用到 `Scanner.Toolkit` 的 `FilePath`；`OSHelper`、`ScriptUtil` 属于未被主流程调用的遗留工具代码（`ScriptUtil` 为 `internal`）。这些涉及 HKLM/HKCR 注册表与 ACL 的方法若被调用通常需要管理员权限。

### 3.4 `Scanner.Visual` —— 分页控件

`PageControl : UserControl`，暴露 `TotalPages / CurrentPage / PageSize` 与 `ClickPageButtonEvent` / `ChangedPageSizeEvent`，提供首页/上一页/下一页/末页四个按钮及「第1/1页」标签。UI 文本（含宋体字体）硬编码在 `PageControl.Designer.cs`。

---

## 4. 一次扫描的完整调用链

```mermaid
flowchart TD
    A[Program.Main] --> B[FormScan 构造]
    B --> C[LoadScanner: new TwainControl]
    C --> D["DataSourceManager: DsmParent(MSG_OpenDSM)"]
    D --> E["GetSourceNames: GetFirst/GetNext DS"]
    E --> F[用户选数据源 / 点击扫描]
    F --> G["SelectSource + StartScanning(ScanSettings)"]
    G --> H["DataSource.Open: 能力协商 Capability → DsCapability"]
    H --> I["DsUserInterface(MSG_EnableDS) 启用数据源"]
    I --> J["WinFormsWindowMessageHook.PreFilterMessage"]
    J --> K["DsEvent(MSG_ProcessEvent) → TWMessage.XFerReady"]
    K --> L["TransferPictures 循环"]
    L --> M["DsImageInfo → DsImageTransfer(DAT_ImageNativeXfer) → hbitmap"]
    M --> N["DsPendingTransfer(MSG_EndXfer) 取剩余页数"]
    N --> O["BitmapRenderer.RenderToBitmap → Bitmap"]
    O --> P["TransferImage 事件 → FormScan 保存 .tiff"]
    P --> Q{pendingTransfer.Count != 0 ?}
    Q -->|是| L
    Q -->|否| R["MSG_Reset + DisableDS/CloseDS"]
    R --> S["ScanningComplete 事件 → FormScan 刷新预览"]
```

分步说明（关键位置）：

1. `Scanner/Program.cs:12` `Main()` → `Application.Run(new FormScan())`。
2. `Scanner/FormScan.cs:87` `LoadScanner()` → `new TwainControl(new WinFormsWindowMessageHook(this))` → `Scanner.Twain/TwainControl.cs:16` 创建 `DataSourceManager` → `DataSourceManager.cs:43` `DsmParent(MSG_OpenDSM)`；失败抛 `TwainException`，但此处被 `FormScan.cs:93-97` 静默吞掉（提示框已被注释）。
3. `TwainControl.GetSourceNames()` → `DataSource.GetAllSources()` 遍历 DS 名称并填充 `cbbDataSource`。
4. `FormScan.cs:139` 扫描按钮：硬编码 `ScanSettings`（ADF/双面/300dpi 彩色/不显示驱动 UI/传输全部页）→ `TwainControl.StartScanning` → `DataSourceManager.StartScan` → `DataSource.Open` 能力协商 → `Enable(MSG_EnableDS)`。
5. TWAIN 消息经 `WinFormsWindowMessageHook` → `DataSourceManager.FilterMessage` → `DsEvent(MSG_ProcessEvent)`；收到 `XFerReady` 时进入 `TransferPictures()`。
6. `DataSourceManager.cs:170-242`：循环调用 `DsImageInfo` / `DsImageTransfer` / `DsPendingTransfer(EndXfer)`，把 DIB 交给 `BitmapRenderer` 渲染，触发 `TransferImage` 事件；`while (pendingTransfer.Count != 0)` 实现 ADF 连续多页；`finally` 中 `MSG_Reset` 清理。
7. `FormScan.cs:99` 事件处理器把每页 `Bitmap` 保存为 `<程序目录>\image\<GUID>.tiff`；`ScanningComplete` 后 `DefaultShowImage()` 重新枚举目录并分页渲染。
8. 关闭时 `DataSource.Close()`（DisableDS → CloseDS）、`DataSourceManager.Dispose()`（CloseDSM）。

---

## 5. 配置、参数与产物

### 5.1 扫描参数（源码硬编码，无配置项）

`Scanner/FormScan.cs:155-168`：

| 参数 | 值 | 含义 |
| --- | --- | --- |
| `UseDocumentFeeder` | `true` | 使用自动进纸器 ADF |
| `UseDuplex` | `true` | 双面扫描 |
| `Resolution` | `ResolutionSettings.ColourPhotocopier` | 300 dpi 彩色（另有 Fax 200dpi、Photocopier 300dpi 灰度预设） |
| `ShowTwainUI` | `false` | 不显示厂商驱动界面 |
| `ShowProgressIndicatorUI` | `false` | 不显示进度指示 |
| `ShouldTransferAllPages` | `true` | 传输全部页（`TransferAllPages = -1`） |
| `Area` | `null` | 不限制扫描区域（默认纸张 `UsLetter`） |
| `Rotation` | `AutomaticRotate=false`, `AutomaticBorderDetection=false` | 关闭自动纠偏与边框检测 |

想改参数只能修改源码后重新编译。`Scanner/Properties/Settings.settings` 是空的 VS 模板，全库无 `appSettings` 节、无 `ConfigurationManager` 调用。

### 5.2 log4net 配置

`Scanner/App.config` 与 `Scanner.Twain/App.config` 内容几乎相同（后者日志文件为 `logs/twain.log`）：

- `configSections` 声明 `log4net` 节（必须置于 `configuration` 第一个节点）。
- 两个 Appender：`ConsoleAppender` + `RollingFileAppender`，输出到 `logs/scanner.log`，`appendToFile=true`、`maximumFileSize=5120KB`、`maxSizeRollBackups=10`、`RollingStyle=Date`，`LevelRangeFilter` 为 DEBUG~FATAL，`root level=ALL`。
- `<startup><supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.2" /></startup>`（仅主工程 App.config 有）。
- `[assembly: log4net.Config.XmlConfigurator(ConfigFileExtension="config", Watch=true)]` 位于 `Scanner/Properties/AssemblyInfo.cs` 与 `Scanner.Twain/Properties/AssemblyInfo.cs`，使配置热更新生效。

### 5.3 运行时产物

| 产物 | 位置 | 说明 |
| --- | --- | --- |
| 扫描图片 | `<exe 目录>\image\<GUID>.tiff` | 由 `FilePath.PathCombine(FilePath.GetAppPath(), "\\image")` 得到，目录不存在时自动创建 |
| 日志 | `<exe 目录>\logs\scanner.log.*`（按日期滚动） | `scanner.log.YYYY-MM-DD`，最大 5 MB / 保留 10 份 |
| 编译输出 | 各工程 `bin\Debug\` / `bin\Release\` | `Scanner.exe`、`Scanner.Twain.dll`、`Scanner.Toolkit.dll`、`Scanner.Visual.dll`、`log4net.dll` + `.config` |
| 中间产物 | 各工程 `obj\` | `.cache`、`.pdb`、`.resources` 等 |

> **重要**：`FormScan` 构造函数会删除 `image` 目录中的**全部文件**，即每次启动程序都会清空上一次的扫描结果。需要留存的扫描件请在关闭程序前复制到别处。

---

## 6. 构建与环境要求

### 6.1 需要的环境

| 项 | 要求 |
| --- | --- |
| 操作系统 | Windows（依赖 `twain_32.dll`、`System.Windows.Forms`、GDI+、注册表） |
| IDE / 工具 | Visual Studio 2017+（`.sln` Format 12.00 / VS17，`ToolsVersion 15.0`）或等效 MSBuild |
| .NET | **.NET Framework 4.5.2 Developer Pack / Targeting Pack**（编译必需）；运行需 .NET Framework 4.5.2 及以上运行时 |
| NuGet | 需要还原 `packages.config` 中的 `log4net 2.0.17` |
| 扫描仪驱动 | 厂商提供的 **32 位 TWAIN 驱动**（见下节） |

### 6.2 平台要求：请按 x86 编译运行（最关键）

- 解决方案只定义了 `Any CPU`，4 个工程的 `PlatformTarget` 也都是 `AnyCPU`（主工程显式写在 `Scanner.csproj:17,27`）。
- 但 `Scanner.Twain/Twain32Native/Twain32Native.cs` 全部 `DllImport("twain_32.dll")` 指向 **32 位 TWAIN DSM**；`Scanner.Twain/Win32Native/BitmapRenderer.cs:41-50` 明确注释 `The following code only works on x86`，并用 `ToInt32()` 计算指针偏移。
- **结论**：在 64 位进程里会加载 DSM 失败或出现位图指针计算错误。建议手动把主工程改为 `x86`（或至少启用 `Prefer 32-bit`），并确保扫描仪安装的是 32 位 TWAIN 驱动；使用 `twaindsm.dll` 的 TWAIN 2.x 64 位驱动不在本代码支持范围内。

### 6.3 构建步骤

```powershell
# 1. 还原 NuGet 包（packages.config 模式，会生成 packages/log4net.2.0.17/）
nuget restore Scanner.sln
#    或 VS 中：工具 → NuGet 包管理器 → 还原 NuGet 程序包

# 2. 使用 VS 打开 Scanner.sln，选择 Debug | Any CPU（建议改为 x86）后生成
#    或用 MSBuild（需 VS 开发人员命令提示符 / MSBuild 在 PATH 中）：
msbuild Scanner.sln /p:Configuration=Release /p:Platform="Any CPU"

# 3. 输出
#    Scanner\bin\Release\Scanner.exe 及同目录下 3 个工程 DLL、log4net.dll、*.config
```

> **注意**：`.gitignore` 忽略了 `**/packages/*` 与 `*.nupkg`，而 `Scanner.csproj` / `Scanner.Twain32.csproj` 的 `HintPath` 直接硬编码为 `..\packages\log4net.2.0.17\lib\net45\log4net.dll`。**干净克隆后必须先执行 NuGet 还原**，否则会因找不到 `log4net.dll` 而编译失败。

### 6.4 项目文件约定

- `.gitattributes` 中只有 `* text=auto` 生效，其余 `diff=csharp`、`merge=binary`、图片 `binary` 等规则均被注释，属于 VS 模板默认状态。
- `.gitignore` 基于 GitHub `VisualStudio.gitignore` 模板，并已补充 IDE / 构建产物忽略规则（见 [§8.8](#88-仓库与工程状态)）。

---

## 7. 运行前置条件与使用说明

1. 安装扫描仪厂商的 **32 位 TWAIN 驱动**，并在 Windows 设备与打印机中确认扫描仪可用（可用系统自带「Windows 传真和扫描」先验证驱动）。
2. 确保 `Scanner.exe` 所在目录**可写**（程序会创建 `image`、`logs` 子目录）。
3. 双击 `Scanner.exe` 启动。程序会枚举数据源并填入左上角下拉框；若下拉框为空，多半是 `twain_32.dll`/DSM 打开失败或未装 32 位驱动（此时程序**不会弹错误提示**，见 [§8](#8-已知限制与注意事项)）。
4. 选择扫描仪，点击「扫描」。ADF 中的纸张会被逐页扫描，每页生成一个 `.tiff`。
5. 扫描完成后切回「微缩视图」，用分页控件或「上一张 / 下一张」浏览；「垂直视图」以单张大图显示当前图片。
6. 关闭程序前，把需要的图片从 `image` 目录复制出去 —— 下次启动会被清空。

**无需**：命令行参数、网络连接、数据库连接、账号登录、管理员权限（除非手动调用 `Scanner.Toolkit` 中写注册表/ACL 的遗留工具方法）。

---

## 8. 已知限制与注意事项

### 8.1 平台与驱动
- `AnyCPU` 配置与 32 位 `twain_32.dll` / x86 位图渲染代码冲突，实际应按 x86 运行（[§6.2](#62-平台要求请按-x86-编译运行最关键)）。
- 未安装数据源时 `DataSourceManager` 抛 `TwainException`，但 `FormScan.LoadScanner()` 的 `catch` 直接 `return`，界面无任何提示，表现为下拉框为空、点击扫描报「没有可用的扫描仪」。

### 8.2 图像输出
- 文件名后缀是 `.tiff`，但 `FormScan.cs:108` 使用 `image.Save(imagePath)` **未显式传 `ImageFormat`**，实际编码由 GDI+ 按扩展名推断；不要假定它是标准多页 TIFF 或带压缩的 TIFF。
- **不支持 PDF / 多页 TIFF 导出**：`ImageFileFormat` 枚举中的 `TiffMulti / Png / Jpeg / Pdf / PdfA / PdfA2` 只是 TWAIN 协议常量，从未在传输流程中使用。
- 每页一个独立文件，文件名为随机 GUID，无排序元数据；`DefaultShowImage()` 按 `DirectoryInfo.GetFiles()` 顺序读取，预览顺序不保证与扫描顺序一致。

### 8.3 UI 与交互
- 「水平视图」Tab 在设计器中存在，但 `FormScan.cs` 的 Tab 切换 `switch` 只处理 case 0/1，该 Tab 无实际功能。
- 分页大小不一致：代码字段 `pageSize = 20`（`FormScan.cs:31`），而 `DefaultPageControl.PageSize` 被设为 `10`（`:48`）；注释也写的是「每达到 10 条数据」。
- 中文界面文本硬编码在 `*.Designer.cs`（`"扫描"`、`"微缩视图"`、`"垂直视图"`、`"水平视图"`、`"下一张"`、`"第1/1页"` 等，字体宋体），`.resx` 仅为 VS 模板内容，无本地化资源，因此界面只有简体中文。
- 扫描过程中窗体被整体 `Enabled = false`，直到 `ScanningComplete` 触发才恢复；若驱动异常未发完成事件，界面可能一直保持禁用。

### 8.4 错误处理与日志
- `DataSource.cs` 有十余处空 `catch { }`（用于「驱动不支持该能力则跳过」），问题会被静默忽略；`DataSourceManager.StartScan` 中 `throw e` 会丢失原始堆栈。
- `FormScan` 构造 `TwainControl` 失败时静默返回；扫描异常仅弹 `MessageBox.Show(ex.Message)`。
- `Capability.cs` 个别 `string.Format` 格式串重复使用 `{1}`，日志里 `_twainType` 不会被打印。
- 日志 `root level=ALL` 且同时输出控制台与文件，长期使用需留意 `logs/` 目录体积（5 MB × 10 份滚动）。

### 8.5 安全与权限
- 无强名称签名、无代码签名、无加密或认证逻辑。
- `Scanner.Toolkit` 中 `ScriptUtil.DisableFips`（写 HKLM）、`RegUrlProtocol`（写 HKCR `lisscanner://`）、`OSHelper.AddSecurityControll2Folder`（给 Everyone/Users 赋 FullControl）若被调用需要管理员权限，且会修改系统级配置；当前主流程未调用，属遗留代码，启用前请评估影响。

### 8.6 配置能力
- 无 `appSettings`、无命令行参数、无外部配置文件（除 log4net 节）。所有扫描参数、输出目录、分页大小都写死在源码里。
- `AutoGenerateBindingRedirects=true` 已设置，但当前 `App.config` 无 `bindingRedirect`（也无 `System.Net.Http` 等版本冲突需求），产物 `.config` 与源文件一致。

### 8.7 工程与质量
- 无单元测试、无 CI/CD、无 CHANGELOG、无安装程序；`Scanner.Visual/bin/Release/` 为空目录（从未构建过 Release 产物）。
- 4 个 `AssemblyInfo.cs` 的 `AssemblyCompany` 为空，`AssemblyCopyright("Copyright ©  2024")` 为 VS 模板遗留；程序集版本统一 `1.0.0.0`。
- TWAIN 应用标识由 `DataSourceManager.DefaultApplicationId` 构造：`Manufacturer/ProductFamily/ProductName = "Scanner"`，版本 1.1，`Language.USA / Country.USA`，Id 由 GUID 生成。

### 8.8 仓库与工程状态
- 各工程 `bin/`、`obj/` 下存在本机 Debug 产物，以及历史滚动日志（`logs/scanner.log.2024-05-19` 等）和 6 个 `.tiff` 扫描样张；这些均已被 `.gitignore` 覆盖，不属于源码。
- `.gitignore` 基于 GitHub `VisualStudio.gitignore` 模板，本次补充了 `.idea/`、`*.iml` / `*.ipr` / `*.iws`、`target/`、`.vscode/`、`.settings/` / `.classpath` / `.project` 等 IDE 与构建产物规则；模板原有的 `.vs/`、`[Bb]in/`、`[Oo]bj/`、`[Oo]ut/`、`*.pdb`、`*.[Cc]ache` 等规则继续生效。

---

## 9. 依赖清单

| 依赖 | 版本 | 引入方式 | 用途 |
| --- | --- | --- | --- |
| .NET Framework | 4.5.2 | 框架引用 | BCL、WinForms、GDI+ |
| `log4net` | 2.0.17（`net45`） | `packages.config` + `HintPath` | 日志记录（主工程与 TWAIN 库） |
| `twain_32.dll` | 系统 / 驱动提供 | P/Invoke `EntryPoint="#1"` | TWAIN 1.9 DSM 入口 |
| `kernel32.dll` / `gdi32.dll` / `user32.dll` | 系统 | P/Invoke | 全局内存、DIB 绘制、消息位置/时间 |

`packages/log4net.2.0.17/` 内含 `.nupkg`、签名文件、图标，以及 7 个目标框架目录（`net20 / net35 / net40 / net40-client / net45 / netstandard1.3 / netstandard2.0`）各一份 `log4net.dll` 与 `log4net.xml`；工程实际引用的是 `lib/net45/log4net.dll`。无其他 NuGet 包、无 `Quartz/RestSharp/Fleck/Newtonsoft.Json/SnmpSharpNet/BouncyCastle` 等依赖，也无 HTTP/WebSocket/SNMP 服务或端口监听（`OSHelper.Ping/PortInActive` 只是未被调用的工具方法）。

---

## 10. 常见问题排查

| 现象 | 可能原因 / 处理 |
| --- | --- |
| 编译时报找不到 `log4net.dll` | `packages/` 被 `.gitignore` 忽略，干净克隆后未还原。执行 `nuget restore Scanner.sln` 或在 VS 中还原 NuGet 包 |
| 编译时报 `.NETFramework,Version=v4.5.2` 找不到 | 未安装 .NET Framework 4.5.2 Developer Pack（Targeting Pack），安装后再构建；`dotnet build` 不适用于旧式 .NET Framework 工程 |
| 启动后数据源下拉框为空 | 未安装 32 位 TWAIN 驱动 / 进程是 64 位导致 `twain_32.dll` 加载失败 / DSM 打开失败被静默吞掉。先用系统「Windows 传真和扫描」验证驱动，再按 x86 编译运行，必要时查看 `logs/scanner.log` |
| `BadImageFormatException` 或位图渲染异常 | 以 64 位进程运行了 x86 专用代码（见 [§6.2](#62-平台要求请按-x86-编译运行最关键)） |
| 点击扫描提示「没有可用的扫描仪」 | 下拉框为空，同上；或 `cbbDataSource.SelectedItem` 为空 |
| 上次扫描的图片不见了 | 设计如此：每次启动清空 `<exe 目录>\image`，见 [§5.3](#53-运行时产物) |
| 找不到 PDF / 合并后的多页文件 | 当前实现每页单独存盘，不生成 PDF 或多页 TIFF |
| 扫描中途窗体一直灰掉 | 驱动未发 `ScanningComplete`；`TransferPictures` 异常路径会走 `CloseDsAndCompleteScanning`，但消息钩子异常时可能不恢复，需重启程序 |
| 日志没有生成 | 检查 `logs/` 目录写权限、`App.config` 中 `configSections` 是否位于第一个节点、`log4net.dll` 是否与 `.exe` 同目录 |

---

## 11. 许可证

本项目采用 **MIT License**，版权所有 `Copyright (c) 2026 wxingfu`。完整条款见 [`LICENSE`](LICENSE)。

---

*本文档基于仓库当前源码静态分析编写；所列行号与行为以实际代码为准，代码变更时请同步更新。*
