﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BatchManager.Services
{
    public interface IAutoSyncService
    {
        Task StartAutoSyncFromSource();
    }
}
