   M S B A M L   `   `   `  ����G  Cgamelauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:gamelauncher.Viewsgamelauncher.Views  5      #    gamelauncher.Views.AdminPanel   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:gamelauncher.Views     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5        N� Title$  
AdminPanel��5      .��5      � $��	150,0,0,0q�5   	   $��Stretch=�5   	   $��Stretch=�5   	      DarkGrey#���   5   	    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $	��11* �6   .+�5   
   �� $��Center=�6    �� Orientation$ 
Horizontal=�62   .+�5      �� $	��150��5      $��40��5        TextBoxMainColor#���  5      #���   5      $��0,0,1,0q�5       �� CornerRadius$ 
8, 0, 0, 8��5      ._�5      �� 
������ $��0q�5        	 Games#���  5"      $<��6pack://application:,,,/fonts/Ubuntu-Medium.ttf #Ubuntu)�5#      $��16(�5$        MainFontColor#���  5%      ��5!      �� *	GoToGames+��5&      �� 5'       �� 
TargetType" ��6    .�5(      ��  �� Property �@�OverridesDefaultStyle �� Value . 6F   �� 5)       � ��
 ���� 6;   �� 5*       � ��
 �����6;   �� 5+       � ��$ 0q�6@   �� 5,       �@�Cursor$
 Hand��67   �� 5-       � �� 5.   "   �� 5/   &    �� 
TargetType" ��66   .R�50   *   ��  �� Padding$ 5q�61   #��z��6=   ._�51   .   �� $��Center=�6?   $��Center=�6\   52   +   53   '   55      56      58      59      �� 5:      $	��150��5;      $��40��5<      #���  5=      $��1,0,0,0q�5>      $ 
0, 8, 8, 0��5?      ._�5@      �� 
������ $��0q�5B       	 Users#���  5D      $<��6pack://application:,,,/fonts/Ubuntu-Medium.ttf #Ubuntu)�5E      $��16(�5F      #���  5G      ��5C      �� *	GoToUsers+��5H      �� 5I      " ��6    .�5J      ��  �@�OverridesDefaultStyle . 6F   �� 5K       � ��
 ���� 6;   �� 5L       � ��
 �����6;   �� 5M       � ��$ 0q�6@   �� 5N       �@�Cursor$
 Hand��67   �� 5O       � �� 5P   "   �� 5Q   &   " ��66   .R�5R   *   �� $ 5q�61   #��z��6=   ._�5S   .   �� $��Center=�6?   $��Center=�6\   5T   +   5U   '   5W      5X      5Z      5[      5\       � 5]   
   -   5        	  �Name$	 AdminNavigate��5^      $��1��5_      $��Stretch=�5`      $��Stretch=�5a      
  � NavigationUIVisibility$
 Hidden=�5b       J� SnapsToDevicePixels . 5c      5d      5e      