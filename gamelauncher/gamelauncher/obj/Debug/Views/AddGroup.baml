   M S B A M L   `   `   `  ����G  Cgamelauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:gamelauncher.Viewsgamelauncher.Views  5      !    gamelauncher.Views.AddGroup   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:gamelauncher.Views     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5        � Title$  AddGroup��5      $	��500��5	      $	��240��5
       � AllowsTransparency . 
������ $��0q�5       � WindowStyle$
 None=�5       � 	Resources 5         WindowFadeIn �� TargetProperty W� From	 W� To g� Duration  InvisibleButtonStyle �� 
TargetType	 �� Property
 �� Value �� 
TargetType �� CornerRadius��%T  &	        &	 x     �� 5   
   .��5      W� $ Opacity�5      $ 0X�5      $ 1X�5      $ 0:0:0.2D�5      5      �� 5   
   " ��6-   .�5      �� 	 � ��

 ���� 6+   �� 5      	 � ��

 ���� 6,   �� 5      	 � ��$
 Stretch=�69   �� 5      	 � ��$
 Stretch=�6;   �� 5      	 �@�Cursor$

 Hand��6'   �� 5      	 � ��$
 14(�6)   �� 5      	 � ��
 5       �� 5!      " ��6&   .R�5"      �� #��z��6!   $ 6��6K   ._�5#      �� $��Center=�6/   $��Center=�6L   5$      5%      5'      5(      5)       � Triggers 5*      :� 5+   
    :� RoutedEvent$ Window.Loaded��6   .[�5,      �� #��[  6   5-      .��5/      � .+�50   
   ��   DarkGrey#���  51      ���#5D4465$��2q�53      $ 20��54      ._�55      �� $��Center=�6    �� Orientation$ Vertical=�64   .+�56      �� $��0,0,0,30q�57      $:��4pack://application:,,,/fonts/Ubuntu-Bold.ttf #Ubuntu)�58      $��24(�59        MainFontColor#���  5:        GroupCreating#���  5;       �� TextAlignment$ Center=�5<      �� 5=      $��45��5>      $��0,0,0,15q�5?      $��Center=�5@      $ 
Horizontal=�5A      .+�5B      �� $��0,0,20,0q�5C      $��Center=�5D      $<��6pack://application:,,,/fonts/Ubuntu-Medium.ttf #Ubuntu)�5E      $��18(�5F      #���  5G        	EnterName#���  5H      �� 5I      $	��220��5J        TextBoxMainColor#���  5K      $ 8��5L      ._�5M      �� $��5q�5N      $��Center=�5O      
������ $��0q�5Q      $<��6pack://application:,,,/fonts/Ubuntu-Medium.ttf #Ubuntu)�5R      $��18(�5S      #���  5T      ��5U      �� *Input+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=�5V      5W      �� 5X      $��Center=�6   $ 
Horizontal=�6:   .+�5Y      �� $��15q�5Z      ���#9319B8$ 6��5\      ��5]      �� 5^      " ��6$    �� Triggers 5_   "   P� 5`   &    P� Property �@J�IsMouseOver P� Value .  N� EnterActions 5a   *   �� 5b   .   .��5c   2   �� .��5d   6   �� $1 +(Border.Background).(SolidColorBrush.Color)�5e   9   	 �� To$ #B824E4X�5f   9   $ 0:0:0.2D�5g   9   5i   3   5j   /    N� ExitActions 5m   *   �� 5n   .   .��5o   2   �� .��5p   6   �� $1 +(Border.Background).(SolidColorBrush.Color)�5q   9   $ #9319B8X�5r   9   $ 0:0:0.2D�5s   9   5t   3   5u   /   5w   '   �� 5x   &    �� Binding 62   �� *
IsActive+ �� ElementName$ 	AddButton�� �� Value	 False6f    5y   *   �� 5z   .   .��5{   2   �� .��5|   6   �� $1 +(Border.Background).(SolidColorBrush.Color)�5}   9   $ #3A3D41X�5~   9   $ 0:0:0.2D�5   9   5�   3   5�   /    5�   *   �� 5�   .   .��5�   2   �� .��5�   6   �� $1 +(Border.Background).(SolidColorBrush.Color)�5�   9   $ #9319B8X�5�   9   $ 0:0:0.2D�5�   9   5�   3   5�   /   5�   '   5�      ._�5�      �� -   5         ��Name$ 	AddButton��5�      $��90��5�      $��35��5�      
������ $��0q�5�       
 Create#���  5�      $<��6pack://application:,,,/fonts/Ubuntu-Medium.ttf #Ubuntu)�5�      
�������#��[ 5�      ��5�      �� *AddGroupCommand+5�      5�      �� 5�      $��15q�5�      ���#9319B8$ 6��5�      ��5�      �� 5�      " ��6$    5�   "   P� 5�   &    �@J�IsMouseOver .  5�   *   �� 5�   .   .��5�   2   �� .��5�   6   �� $1 +(Border.Background).(SolidColorBrush.Color)�5�   9   $ #B824E4X�5�   9   $ 0:0:0.2D�5�   9   5�   3   5�   /    5�   *   �� 5�   .   .��5�   2   �� .��5�   6   �� $1 +(Border.Background).(SolidColorBrush.Color)�5�   9   $ #9319B8X�5�   9   $ 0:0:0.2D�5�   9   5�   3   5�   /   5�   '   5�      ._�5�      �� $��90��5�      $��35��5�      
������ $��0q�5�        Back#���  5�      $<��6pack://application:,,,/fonts/Ubuntu-Medium.ttf #Ubuntu)�5�      
�������#��[ 5�      ��5�      �� *CloseCommand+5�      5�      5�      5�      5�      5�      5�      