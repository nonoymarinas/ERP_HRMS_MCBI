﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    public interface ISaveSigninData
    {

        public Task<ReturnSaveSigninDataModel> SaveSigninData();
    }
}