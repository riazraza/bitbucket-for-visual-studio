﻿using GitClientVS.Contracts.Interfaces.Views;

namespace GitClientVS.Contracts.Interfaces
{
    public interface IView
    {
        object DataContext { get; set; }
    }
}
