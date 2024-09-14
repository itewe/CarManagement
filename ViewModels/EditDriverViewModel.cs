using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

public class EditDriverViewModel
{
    public int VehicleId { get; set; }
    public int SelectedDriverId { get; set; }
    public SelectList Drivers { get; set; }
}
