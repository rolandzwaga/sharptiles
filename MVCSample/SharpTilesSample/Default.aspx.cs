﻿using System;
using System.Web.UI;

namespace SharpTilesSample
{
    public class _Default : Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Home");
        }
    }
}