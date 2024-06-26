﻿using System;
using System.Collections.Generic;

namespace Scanner.Twain
{
    public class TwainControl
    { 
        private DataSourceManager _dataSourceManager;

        public TwainControl(IWindowsMessageHook messageHook)
        {
            ScanningComplete += delegate { };

            TransferImage += delegate { };

            _dataSourceManager = new DataSourceManager(DataSourceManager.DefaultApplicationId, messageHook);

            _dataSourceManager.ScanningComplete += delegate (object sender, ScanningCompleteEventArgs args)
            {
                ScanningComplete(this, args);
            };

            _dataSourceManager.TransferImage += delegate (object sender, TransferImageEventArgs args)
            {
                TransferImage(this, args);
            };

        }


        /// <summary>
        /// Notification that the scanning has completed.
        /// </summary>
        //public event EventHandler<ScanningStartEventArgs> ScanningStart;

        public event EventHandler<ScanningCompleteEventArgs> ScanningComplete;

        //public event EventHandler<ScanningExceptionArgs> ScanningException;

        //public event EventHandler<ScanningEventArgs> ScanningEvent;

        //public event EventHandler<TransferReadyEventArgs> TransferReady;

        public event EventHandler<TransferImageEventArgs> TransferImage;

        //public event EventHandler<TransferFileEventArgs> TransferFile;


        /// <summary>
        /// Starts scanning.
        /// </summary>
        public void StartScanning(ScanSettings settings)
        {
            _dataSourceManager.StartScan(settings);
        }

        /// <summary>
        /// Shows a dialog prompting the use to select the source to scan from.
        /// </summary>
        public void SelectSource()
        {
            _dataSourceManager.SelectSource();
        }

        /// <summary>
        /// Selects a source based on the product name string.
        /// </summary>
        /// <param name="sourceName">The source product name.</param>
        public void SelectSource(string sourceName)
        {
            var source = DataSource.GetSource(sourceName, _dataSourceManager.ApplicationId, _dataSourceManager.MessageHook);
            _dataSourceManager.SelectSource(source);
        }

        /// <summary>
        /// Gets the product name for the default source.
        /// </summary>
        public string GetDefaultSourceName()
        {
            using (var source = DataSource.GetDefault(_dataSourceManager.ApplicationId, _dataSourceManager.MessageHook))
            {
                return source.SourceId.ProductName;
            }
        }

        /// <summary>
        /// Gets a list of source product names.
        /// </summary>
        public List<string> GetSourceNames()
        {
            var result = new List<string>();
            var sources = DataSource.GetAllSources(_dataSourceManager.ApplicationId, _dataSourceManager.MessageHook);
            foreach (DataSource source in sources)
            {
                result.Add(source.SourceId.ProductName);
                source.Dispose();
            }

            return result;
        }


    }
}
