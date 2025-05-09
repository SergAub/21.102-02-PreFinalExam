﻿using _21._102_02_PreFinalExam.dbModel;
using _21._102_02_PreFinalExam.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21._102_02_PreFinalExam.ViewModel
{
    public class RegistrationData
    {
        public Registration registration { get; set; }
        public Clients client { get; set; }

        public string ClientPhoto{
            get
            {
                if(client.Photo == null || client.Photo.Length == 0)
                {
                    return "/Resources/User.png";
                }
                return client.Photo;
            }
        }
    }
}
