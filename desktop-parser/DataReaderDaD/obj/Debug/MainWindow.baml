   M S B A M L   `   `   `  ????H  DDataReaderDaD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:DataReaderDaDDataReaderDaD  5   	   C&clr-namespace:DataReaderDaD.PropertiesDataReaderDaD.Properties  5   	       DataReaderDaD.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:DataReaderDaD     5   	   =
Properties&clr-namespace:DataReaderDaD.Properties     5   	   -   5          ?Name$  MainWindow1??5   C    ? Title$ NinerFi Analytics??5	   	   $	??600??6#   $	??800??60   ???	#FF64695F J? 	AllowDrop . $	??800??6?   $	??550??6?   $	??800??6?    ? 	Resources 5
         ProgressBar.Progress ?? Color  ProgressBar.Background  ProgressBar.Border  ProgressBarStyle1 ?? 
TargetType ?? Property ?? Value ?? 
TargetType	 ?Name'  !System.Windows.VisualStateManager
   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard g? RepeatBehavior ?? 
TargetName ?? TargetProperty9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O? KeyTime O? Value8  2System.Windows.Media.Animation.EasingPointKeyFrame $? KeyTime $? Value ?? CornerRadius ??Name ?? RadiusX ?? RadiusY J? RenderTransformOrigin ?? Triggers P? Property P? Value ?? 
TargetName ?? Angle??%v  &	        &	 #     &	 F     &	 i     7   7  7  ?? 5   
   $ 	#FF06B025??67   ?? 5   
   $ 	#FFE6E6E6??69   ?? 5   
   $ 	#FFBCBCBC??65   ?? 5   
   " ?6*   .?5      ??  ? ??8   6+   ?? 5       ? ??8  6+   ?? 5       ? ??8  6,   ?? 5       ? ??$ 1q?60   ?? 5       ? ?? 5      ?? 5      " ?6&   .R?5      ? $	 TemplateRoot??6   
 5        5   "   CommonStates??. 5   &     Determinate??62     5   &   Indeterminate??. 5   *   ?? $ Forever??65   .??5   .   U? $ 	Animation??6L   $V P(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)?6n   .\?5   2     $ 0??6G   $
 0.25Q?6S     5   2   $ 0:0:1??6G   $
 0.25Q?6W     5   2   $ 0:0:2??6G   $
 0.25Q?6W   5    /   *? 5!   .   $ 	Animation??6K   $' !(UIElement.RenderTransformOrigin)?6m   .'?5"   2     $ 0??6F   $ -0.5,0.5&?6R     5#   2   $ 0:0:1??6F   $ 0.5,0.5&?6V     5$   2   $ 0:0:2??6F   $ 1.5,0.5&?6V   5%   /   5&   +   5'   '   5(   #   .+?5*      ?? #??z??6%   #??z??6O   #??z??6{   $ 10??6?   ?? 5+      $ 
PART_Track??6(   ? 5,      $	 PART_Indicator??6#   }?. $
??Left=?6O   .+?5-   "   ?? $ 	Indicator??6,   #??z??6?   $ 10??6c   $ 10??6p   ?? 5.   "   $ 	Animation??6,   #??z??6?   $ 0.5,0.5&?6c   $ 10??6?   $ 10??6?   z?5/   &   V? 50   *   .~?51   .   ?? ?? 52   .   ?? 53   .   T? 54   .   55   +   57   #   58      59       5:      P? 5;       ?@?Orientation$ Vertical=?6=   . ?5<   "   ?? $ TemplateRoot??6D    ?@?LayoutTransform 5=   &   ?? 5>   *   $	 -90Q?6:   5@   #   5A      P? 5B       ?@?IsIndeterminate . . ?5C   "   ?? $ 	Indicator??6?    ? y?$ 	Collapsed=?6V   5D      5F      5H      5I      5J      .??5K      ? 
???????$??Stretch=?6   $	??NaN??6<   $??0,0,0,0q?6I   $??Stretch=?6Z   $	??NaN??6v    ? RowDefinitions 5L   
   ?? 5M      $??70 ?6   ?? 5N      ?? 5O      $??52 ?6   ?? 5P      $??32 ?6   ?? 5Q      $??70.04 ?6    ? ColumnDefinitions 5S   
   ?? 5T      $	??400 ?6   ?? 5U      $	??400 ?6   .+?5W   
   ?? -   5          ??Name$  
TitleLabel??5W      $??NinerFi Analytics??6$   $??41,13,10,13q?6@   $??30(?6U   $
??Bold"?6c   $??	Limelight)?6u   ???	#FF015035$??Center=?6?   $??Center=?6?   ??6?   Z? 6?   &   System.Windows.Controls.DataGrid  5Y   
   -   5        !  Name$! FilesDataGrid??5Y   ?     DataGridRowStyle1"   RowStyle#" ?  6     DataGridColumnHeaderStyle1#   ColumnHeaderStyle## ?  6B   $??41,10,41,10q?6?   $??2??6?   $??1??6?   $??Stretch=?6?   $??Stretch=?6  $	??NaN??60  $	??NaN??6=  $	??250??6v  $	??250??6?  $   SelectionUnit$$ FullRow=?6?  %   RowDetailsVisibilityMode$% Visible=?6?  ??6?   ?? ??6I  Z? ??6`  Z? &   Columns& 5Z      4  .System.Windows.Controls.DataGridTemplateColumn  5[      ,  &System.Windows.Controls.DataGridColumn'   Width5	  /System.Windows.Controls.DataGridLengthConverter$' 24	 6)   (   CellTemplate( 5\      ?? 5]      .R?5^      ?? -   5        .??5_   "   ?? $
??Fill=?6(   $??20??67   $??20??6C   ??5`   &   ?? 5a   *   " ??60   .?5b   .   ??  ? ??$ Icons/cancel_green.png??6G   ) ?? Triggers) 5c   .   P? 5d   2    ?@J?IsMouseOver . . ?5e   6   ??  ? ??$ Icons/cancel_red.png??6O   5f   3   5h   +   5j   #   ??5k   "   ?? 5l   &   " ??66   .R?5m   *   ?? #??z??6;    5n   *   P? 5o   .    ?@J?IsMouseOver . . ?5p   2   ??  ?@?Cursor$
 Hand??6K   5q   /   P? 5s   .    ?@J?IsMouseOver .  . ?5t   2   ??  ?@?Cursor$ Arrow??6K   5u   /   5w   '   5y      5z      5|      0
  *System.Windows.Controls.DataGridTextColumn
  5}      *   Header$* 	File Name??6%   1  +System.Windows.Controls.DataGridBoundColumn+   Binding+ 68   ?? *Name+
  5~      $* 	File Size??6%   + 68   ?? *Length+, ?? StringFormat$, {0} MB??
  5      $* File Location??6%   + 6<   ?? *DirectoryName+5?      ? 5?   
   -   5        - ?Name$- DataProcessingProgress??5?   B   #???  6   $??	41,0,41,0q?6b   $??2??6u   $??Center=?6?   $??25??6?   $??2??6?   ???	#FF015035???	#FF015035$??Stretch=?6?   $??Center=?6  $	??NaN??63  ???	#FFA39665$??2,2,2,2q?6V  ?? 5?   
   -   5        $  OutputTextBox??5?      $??3??6'   $??1??64   $??0,0,41,0q?6D   ?? 5?   
   -   5        . ??Name$. StartButton??5?      $??4??6&   $??	213,0,0,0q?63   $??1??6F   $??Start??6V   
??????????	#FF015035???	#FFA39665$??32??6?   $??32??6?   $	??150??6?   $	??150??6?   $
??Left=?6?   $??Center=?6  $??32??64  $	??150??6@  ?? 5?   
   -   5        $. CancelButton??5?      $??4??6'   $??37,0,0,0q?64   $??Cancel??6F   
??????????	#FF015035???	#FFA39665$??32??6?   $??32??6?   $	??150??6?   $	??150??6?   $
??Left=?6?   $??Center=?6  $??32??6&  $	??150??62  5?      5?      