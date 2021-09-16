<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/134076739/21.1.5%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E1520)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [VGridControlDetailViewItem.cs](./CS/WinSolution.Module.Win/VGridControlDetailViewItem.cs) (VB: [VGridControlDetailViewItem.vb](./VB/WinSolution.Module.Win/VGridControlDetailViewItem.vb))
<!-- default file list end -->
# OBSOLETE - How to use VGridControl instead of the LayoutControl in the DetailView


<p><strong>========================================</strong></p>
<p><strong>This example is now obsolete and is no longer maintained, because this custom VGridControl layout implementation is now quite rare. Second, it is better to implement a custom LayoutManager for this task, rather than embedding a fake ViewItem into the existing DetailView layout.</strong></p>
<p><strong>========================================</strong><br>In this example, I have created a custom ViewItem based on the VGridControl.<br> The idea is to remove all layout items that you want to show in the vertical grid and instead of them place our single ViewItem onto the layout.<br> Then, all the editors for business object properties will be generated with the help of the VGridControl.</p>
<p>To use this custom item, you should customize the required DetailView in the Model Editor as shown in the WinSolution.Module.Win\Model.DesignedDiffs.xafml file.</p>
<p><strong>See Also:</strong><br><a href="https://www.devexpress.com/Support/Center/p/S33593">How to integrate a Vertical Grid control into an XAF application</a></p>

<br/>


