   M S B A M L   `   `   `  ����G  Cgamelauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:gamelauncher.Viewsgamelauncher.Views  5      !    gamelauncher.Views.EditGame   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:gamelauncher.Views     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5        � Title$  EditGame��5      $	��950��5	      $	��585��5
       � AllowsTransparency . 
������  � WindowStyle$
 None=�5       � 	Resources 5         WindowFadeIn �� TargetProperty W� From	 W� To g� Duration  CustomThumbStyle �� 
TargetType	 �� Property
 �� Value �� 
TargetType  TopBarColor �� CornerRadius  CustomScrollBar ]�Name � Maximum �� ViewportSize � Value ]� DecreaseRepeatButton ]� Thumb ]� IncreaseRepeatButton��%�  &	        &	 x     &	 \    7  �� 5   
   .��5      W� $ Opacity�5      $ 0X�5      $ 1X�5      $ 0:0:0.2D�5      5      �� 5   
   " n�6)   .�5      �� 	 � ��$

 Auto��6'   �� 5      	 � ��$
 5��6&   �� 5      	 � ��
 5      �� 5      " n�6&   .R�5      �� #���  6!   $ 2��6L   5      5       5!      �� 5#   
   " ��6(   .�5$      �� 	 � ��$
 4��6&   �� 5%      	 � ��$

 Auto��6'   �� 5&      	 � ��
 5'      �� 5(      " ��6&   .R�5)      � .+�5*      ]� $ 
PART_Track��5+   !   ��. #��z 5-   !   #��z 5.   !   #��z 5/   !    50   "   �� 51   &   $��0��52   )   $��ScrollBar.LineDownCommand��53   )   ��54   )   Z�  56   "   n� 57   &   8��  6,    59   "   �� 5:   &   $��0��5;   )   $��ScrollBar.LineUpCommand��5<   )   ��5=   )   Z� 5?      5@      5A      5C      5D      5F       � Triggers 5G      :� 5H   
    :� RoutedEvent$ Window.Loaded��6   .[�5I      �� #��[  6   5J      .��5L      �� #���  5M   	   ���#5D4465$��2q�5O   	   $ 20��5P   	    J� Effect 5Q   
   3  -System.Windows.Media.Effects.DropShadowEffect  5R         
BlurRadius$ 15Q�5S         Opacity$
 0.35Q�5T         ShadowDepth$ 0Q�5U         Color$ Black��5V      ._�5X   
   � .+�5Y      ��  �� Orientation$ Vertical=�6   .+�5Z      �� -   5         ��Name$ Title��5[      $��50��5\      $��0,20,0,0q�5]      $��Center=�5^      $	��Top=�5_      $<��6pack://application:,,,/fonts/Ubuntu-Medium.ttf #Ubuntu)�5`      $��18(�5a        MainFontColor#���  5b      ��5c      �� 5d      " ��6     �� Triggers 5e      �� 5f   "    �� Binding 6.   �� *IsEdit+ �� Value True6I   .a�5g   &   �� 	 � ��
 6=   �� *WindowTitle+5h   #   �� 5i   "    6.   �� *IsEdit+	 False6I   .a�5j   &   �� 	 � ��  
CreateGame#
 �  6=   5k   #   5m      5o      �� 5p      -   5        $ 
UnderTitle��5q      $��50��5r      $��0,5,0,0q�5s      $��Center=�5t      $	��Top=�5u      $<��6pack://application:,,,/fonts/Ubuntu-Medium.ttf #Ubuntu)�5v      $��16(�5w      #���  5x      ��5y      �� 5z      " ��6     5{      �� 5|   "    6.   �� *IsEdit+ True6I   .a�5}   &   �� 	 � y�$
 Visible=�6C   5~   #   �� 5   "    6.   �� *IsEdit+	 False6I   .a�5�   &   �� 	 � y�$
 Hidden=�6C   5�   #   5�      ��5�      `� 5�        �� StringFormat$  ID: {0}, {1}��6'   .1�5�      �� ! �� Path$! Id�6&   �� 5�      $! Created�6&   5�      5�      �� 5�      $��Center=�6   $ 
Horizontal=�6:   .+�5�      �� $��
0,-10,10,0q�5�      $��Center=�5�      $��Center=�5�      $ Vertical=�5�       5�        TextBlockStyle  TextBoxStyle  TextBoxBorderStyle 	 TextBoxMainColor��%�  &	       &	 "    &	 U    �� 5�      " ��6;   .�5�   "   �� 	 � ��$
 	0,55,0,55q�6;   �� 5�   "   	 � ��#
 �  6?   �� 5�   "   	 � ��$<
 6pack://application:,,,/fonts/Ubuntu-Medium.ttf #Ubuntu)�6?   �� 5�   "   	 � ��$
 16(�6=   �� 5�   "   	 �@��TextAlignment$
 Right=�6B   5�      �� 5�      " ��69   .�5�   "   �� 	 � ��

 ���� 6?   �� 5�   "   	 � ��$
 0q�6D   �� 5�   "   	 � ��$
 Center=�6M   �� 5�   "   	 � ��$<
 6pack://application:,,,/fonts/Ubuntu-Medium.ttf #Ubuntu)�6?   �� 5�   "   	 � ��#
 �  6?   �� 5�   "   	 � ��$
 16(�6=   5�      �� 5�      " ��6?   .�5�   "   �� 	 �@��Padding$
 5q�6<   �� 5�   "   	 � ��$	
 300��6:   �� 5�   "   	 � ��$
 38��6;   �� 5�   "   	 � ��$
 
10,10,0,10q�6;   �� 5�   "   	 � ��#
 � 	 6?   �� 5�   "   	 �@��CornerRadius$
 8��6A   5�      5�      .+�5�      �� $��Right=�6%   $ 
Horizontal=�6A   .+�5�      ��  

 Title:#��� 
 6(   ��5�   "   �� 5�   &   " �� BasedOn#" [ 6,   " ��6V   .�5�   *   �� 	 � ��$
 0,15,0,0q�6C   5�   '   5�      �� 5�      #��[ 6%   ._�5�   "   �� #��[ 6*   ��6P   �� *Title+# �� UpdateSourceTrigger$# PropertyChanged=�$ �� Mode$$ TwoWay=�5�      5�      �� 5�      $��Right=�6%   $ 
Horizontal=�6A   .+�5�      ��   Description:#���  6(   ��5�   "   �� 5�   &   #" [ 6,   " ��6V   .�5�   *   �� 	 � ��$
 0,15,0,0q�6C   5�   '   5�      �� 5�      ��5�   "   �� 5�   &   #" [ 6,   " ��6Z   .�5�   *   �� 	 � ��$	
 180��6C   5�   '   ._�5�   "   �� ��6*   �� *Description+$# PropertyChanged=�$$ TwoWay=�.��6y   ��5�   &   �� 5�   *   #" [ 60   " ��6X   .�5�   .   �� 	 �@��TextWrapping$

 Wrap=�6M   �� 5�   .   	 �@��AcceptsReturn
 . 6N   �� 5�   .   #	 �@��VerticalScrollBarVisibility$

 Auto=�6\   �� 5�   .   %	 �@��HorizontalScrollBarVisibility$
 Disabled=�6^   �� 5�   .   	 � ��$	
 Top=�6Y   �� 5�   .   	 � ��
 5�   2   �� 5�   6   " ��6F   .R�5�   :   �� % ��Name$% PART_ContentHost��5�   =   $& �� HorizontalScrollBarVisibility#��z& 5�   =   "' �� VerticalScrollBarVisibility#��z' 5�   =    5�   >   ��%+   '��       7  �� 5�   B   8"   " ��6s   5�   ?   5�   ;   5�   7   5�   /   5�   +   .��5�   5   5�   #   5�      5�      �� 5�      $��Right=�6%   $ 
Horizontal=�6A   .+�5�      ��  
 Price:#���  6(   ��5�   "   �� 5�   &   #" [ 6,   " ��6V   .�5�   *   �� 	 � ��$
 0,15,0,0q�6C   5�   '   5�      �� 5�      #��[ 6%   ._�5�   "   �� #��[ 6*   ��6P   �� *Price+$# PropertyChanged=�$$ TwoWay=�$  {0:N2}��5�      5�      �� 5�      $��5,0,0,0q�5�      $��Right=�5�      $��Center=�5�      $ 
Horizontal=�5�      .+�5�      ��  
 Cover:#���  6(   ��5�   "   �� 5�   &   #" [ 6,   " ��6V   .�5�   *   �� 	 � ��$
 0,10,0,0q�6C   5�   '   5�      �� 5�      $��Right=�5�   !   $��Center=�5�   !   $ 
Horizontal=�5�   !   .+�5�   "   �� ��5�   &   �� 5   *   #" [ 60   " ��6^   .�5  .   �� 	 � ��$	
 240��6F   �� 5  .   	 � ��$

 Left=�6T   �� 5  .   	 � ��$
 10,0,0,0q�6G   5  +   ._�5  &   �� ��6.   �� *	ImagePath+$$ TwoWay=�$# PropertyChanged=�.��6{   ��5  *   �� 5  .   #" [ 64   " ��6\   .��5	  9   5
  '   5  #   �� 5  "   ��5  &   �� 5  *   #" [ 60   " ��6^   .�5  .   �� 	 � ��$
 50��6F   �� 5  .   	 � ��$
 30��6G   �� 5  .   	 � ��$
 Center=�6R   �� 5  .   	 �@��Padding$
 3q�6H   5  +   ._�5  &   �� ��6-   �� *BrowseImageCommand+��5  *   �� 5  .   " ��64   .�5  2   �� 	 �@�OverridesDefaultStyle
 . 6Z   �� 5  2   	 � ��

 ���� 6O   �� 5  2   	 � ��

 �����6O   �� 5  2   	 � ��$
 0q�6T   �� 5  2   	 �@�Cursor$

 Hand��6K   �� 5  2   	 � ��
 5  6   �� 5  :   " ��6J   .R�5   >   �� ( �� Padding$( 5q�6E   #��z��6Q   ._�5!  B   �� $��Center=�6S   $��Center=�6p   5"  ?   5#  ;   5%  3   5&  /   .��5(  *   �� $��25��5)  -   $��Center=�5*  -   $��/img/Three_dots.png��5+  -   5,  '   5-  #   5.     5/     50     �� 51     $��0,0,0,0q�52     $��Center=�53     $ 
Horizontal=�54     .+�55     �� $ Vertical=�6%    56     ��%^  &	       &	 3    &	 I    �� 57  "   " ��6=   .�58  &   �� 	 � ��

 ���� 6C   �� 59  &   	 � ��$
 0q�6H   �� 5:  &   	 � ��$
 Center=�6Q   �� 5;  &   	 � ��$<
 6pack://application:,,,/fonts/Ubuntu-Medium.ttf #Ubuntu)�6C   �� 5<  &   	 � ��#
 �  6C   �� 5=  &   	 � ��$
 16(�6A   5>  #   �� 5?  "   " ��6C   .�5@  &   �� 	 �@��Padding$
 5q�6@   �� 5A  &   	 � ��$	
 245��6>   �� 5B  &   	 � ��$
 38��6?   �� 5C  &   	 � ��$
 
10,10,0,10q�6?   �� 5D  &   	 � ��#
 � 	 6C   �� 5E  &   	 �@��CornerRadius$
 8��6E   5F  #   �� 5G  "   " ��6?   .�5H  &   �� 	 � ��#
 �  6C   �� 5I  &   	 � ��$<
 6pack://application:,,,/fonts/Ubuntu-Medium.ttf #Ubuntu)�6C   �� 5J  &   	 � ��$
 16(�6A   �� 5K  &   	 �@��TextAlignment$
 Right=�6F   5L  #   5M     .+�5N     �� $��Right=�6)   $ 
Horizontal=�6E   .+�5O  "   ��   Genres:#���  6,   ��5P  &   �� 5Q  *   #" [ 60   " ��6Z   .�5R  .   �� 	 � ��$
 0,10,0,0q�6G   5S  +   5U  #   �� 5V  "   $	��300��5W  %   $	��185��5X  %   $��10,5,0,0q�5Y  %   #��� 	 5Z  %   $ 10��5[  %   ��5\  &   �� 5]  *   " ��60   .�5^  .   �� 	 � ��$
 0q�6P   5_  +   ._�5a  &   �� $��0q�5b  )   $��Center=�5c  )   $��Center=�5d  )   $��0q�5e  )   $<��6pack://application:,,,/fonts/Ubuntu-Medium.ttf #Ubuntu)�5f  )   .��5g  *   �� $
��Auto=�67   .��5h  .   �� ��6;   �� *	AllGenres+��5i  2   �� 5j  6   .R�5k  :   � $	��180��6D   ) � Orientation$) Vertical=�6T   5l  7   ��5n  2   �� 5o  6   .R�5p  :   �� $��5q�5q  =   $��10(�5s  =   #���  5t  =   ��5r  =   �� *Name+* d� 	IsChecked* 5u  =   �� *
IsSelected+$$ TwoWay=�5v  7   5x  /   5y  +   5z  '   5{  #   5|     �� 5~     $��Right=�6)   $ 
Horizontal=�6E   .+�5  "   ��   
Platforms:#���  6,   ��5�  &   �� 5�  *   #" [ 60   " ��6Z   .�5�  .   �� 	 � ��$
 	0,20,10,0q�6G   5�  +   5�  #   �� 5�  "   $	��300��5�  %   $��40��5�  %   $��0,15,0,0q�5�  %   #��� 	 5�  %   $ 10��5�  %   ��5�  &   �� 5�  *   " ��60   .�5�  .   �� 	 � ��$
 0q�6P   5�  +   ._�5�  &   �� $��0q�5�  )   $��Center=�5�  )   $��Center=�5�  )   $��0q�5�  )   $<��6pack://application:,,,/fonts/Ubuntu-Medium.ttf #Ubuntu)�5�  )   .��5�  *   �� $
��Auto=�67   .��5�  .   �� ��6;   �� *AllPlatforms+��5�  2   �� 5�  6   .R�5�  :   � $	��180��6D   $) 
Horizontal=�6T   5�  7   ��5�  2   �� 5�  6   .R�5�  :   �� $��5q�5�  =   $��13(�5�  =   #���  5�  =   ��5�  =   �� *Name+* 5�  =   �� *
IsSelected+$$ TwoWay=�5�  7   5�  /   5�  +   5�  '   5�  #   5�     �� 5�     $��Right=�6)   $ 
Horizontal=�6E   .+�5�  "   ��  	 Size:#���  6,   ��5�  &   �� 5�  *   #" [ 60   " ��6Z   .�5�  .   �� 	 � ��$
 0,25,0,0q�6G   5�  +   5�  #   �� 5�  "   ��5�  &   �� 5�  *   #" [ 60   " ��6^   .�5�  .   �� 	 � ��$
 
10,15,0,10q�6G   �� 5�  .   	 � ��$	
 300��6F   5�  +   ._�5�  &   �� #��[ 6.   ��6T   �� *Size+$# PropertyChanged=�$$ TwoWay=�$  {0:N2}��5�  #   5�     �� 5�     $��5,0,0,0q�5�  !   $��Right=�5�  !   $��Center=�5�  !   $ 
Horizontal=�5�  !   .+�5�  "   �� $��Center=�5�  %   $��Center=�5�  %   $ 
Horizontal=�5�  %   .+�5�  &   ��  	 Logo:#���  60   ��5�  *   �� 5�  .   #" [ 64   " ��6^   .�5�  2   �� 	 � ��$
 0,10,0,0q�6K   5�  /   5�  '   �� 5�  &   ��5�  *   �� 5�  .   #" [ 64   " ��6b   .�5�  2   �� 	 � ��$	
 240��6J   �� 5�  2   	 � ��$

 Left=�6X   �� 5�  2   	 � ��$
 10,0,0,0q�6K   5�  /   ._�5�  *   �� ��62   �� *LogoImagePath+$$ TwoWay=�$# PropertyChanged=�.��6�   ��5�  .   �� 5�  2   #" [ 68   " ��6`   .��5�  =   5�  +   5�  '   �� 5�  &   ��5�  *   �� 5�  .   #" [ 64   " ��6b   .�5�  2   �� 	 � ��$
 50��6J   �� 5�  2   	 � ��$
 30��6K   �� 5�  2   	 � ��$
 Center=�6V   �� 5�  2   	 �@��Padding$
 3q�6L   5�  /   ._�5�  *   �� ��61   �� *BrowseLogoImageCommand+��5�  .   �� 5�  2   " ��68   .�5�  6   �� 	 �@�OverridesDefaultStyle
 . 6^   �� 5�  6   	 � ��

 ���� 6S   �� 5�  6   	 � ��

 �����6S   �� 5�  6   	 � ��$
 0q�6X   �� 5�  6   	 �@�Cursor$

 Hand��6O   �� 5�  6   	 � ��
 5�  :   �� 5�  >   " ��6N   .R�5�  B   �� $( 5q�6I   #��z��6U   ._�5�  F   �� $��Center=�6W   $��Center=�6t   5�  C   5�  ?   5�  7   5�  3   .��5�  .   �� $��25��5�  1   $��Center=�5�  1   $��/img/Three_dots.png��5�  1   5   +   5  '   5  #   5     5     5     5     5     �� 5     $��0,0,0,20q�5	     $��Center=�5
     $��Bottom=�5     $ 
Horizontal=�5      5       MenuButtonBorder+ �� Color, P� Property- P� Value. N� EnterActions	/ �� To0 N� ExitActions  
MenuButton��%�  &	       &	 �    �� 5     " ��65   .�5     �� 	 � ��

 �  ��67   �� 5     	 � ��$
 10q�63   �� 5     	 �@��CornerRadius$
 6��69   �� 5     	 � ��
 5     �� 5  "   $+ #9319B8��62   5     �� 5     	 � ��$
 35��63    5     P� 5     , �@J�IsMouseOver- . . 5  "   �� 5  &   .��5  *   �� .��5  .   �� $1 +(Border.Background).(SolidColorBrush.Color)�5  1   $/ #B824E4X�5  1   $ 0:0:0.2D�5   1   W� 5"  .   $/ )(RenderTransform).(ScaleTransform.ScaleX)�5#  1   $
 1.05X�5$  1   $ 0:0:0.2D�5%  1   W� 5&  .   $/ )(RenderTransform).(ScaleTransform.ScaleY)�5'  1   $
 1.05X�5(  1   $ 0:0:0.2D�5)  1   5*  +   5+  '   0 5.  "   �� 5/  &   .��50  *   �� .��51  .   �� $1 +(Border.Background).(SolidColorBrush.Color)�52  1   $/ #9319B8X�53  1   $ 0:0:0.2D�54  1   W� 55  .   $/ )(RenderTransform).(ScaleTransform.ScaleX)�56  1   $ 1X�57  1   $ 0:0:0.2D�58  1   W� 59  .   $/ )(RenderTransform).(ScaleTransform.ScaleY)�5:  1   $ 1X�5;  1   $ 0:0:0.2D�5<  1   5=  +   5>  '   5@     5B     �� 5C     " ��6/   .�5D     �� 	 � ��$<
 6pack://application:,,,/fonts/Ubuntu-Medium.ttf #Ubuntu)�67   �� 5E     	 � ��

 �����67   �� 5F     	 �@�OverridesDefaultStyle
 . 6B   �� 5G     	 � ��

 ���� 67   �� 5H     	 � ��$
 0q�6<   �� 5I     	 �@�Cursor$

 Hand��63   �� 5J     	 � ��
 5K     �� 5L  "   " ��62   .R�5M  &   �� $( 5q�6-   #��z��69   ._�5N  *   �� $��Center=�6;   $��Center=�6X   5O  '   5P  #   5R     5S     5T     .+�5U     �� ��5V     �� 5W     #" [ 6    " ��6L    5X     �� 5Y  "    6.   �� *IsEdit+	 False6I   . 5Z  &   �� 5[  *   .��5\  .   �� .��5]  2   �� $1 +(Border.Background).(SolidColorBrush.Color)�5^  5   $/ #3A3D41X�5_  5   $ 0:0:0.2D�5`  5   5b  /   5c  +   0 5e  &   �� 5f  *   .��5g  .   �� .��5h  2   �� $1 +(Border.Background).(SolidColorBrush.Color)�5i  5   $/ #9319B8X�5j  5   $ 0:0:0.2D�5k  5   5l  /   5m  +   5o  #   5q     ._�5s     �� $��10q�5t     
������ $��0q�5v     ��5w     �� *AddImagesCommand+��5x     �� 5y     #" [ 6$   " ��6J   .�5z  "   �� 	 � ��  AddingImages#
 �  6<   5{     5}     5~     �� 5     ��5�     �� 5�     #" [ 6    " ��6L   .�5�     �� 	 � ��$
 4,0,4,0q�6@    5�     �� 5�  "    6.   �� *
IsActive+ True6K   .a�5�  &   �� 	 � ��

 � � �6D   5�  #   �� 5�  "    6.   �� *
IsActive+	 False6K   .a�5�  &   �� 	 � ��

 �  ��6D   5�  #   5�     ._�5�     �� $��10q�5�     
������ $��0q�5�     ��5�     �� *ChangeActivityState+��5�     �� 5�     #" [ 6$   " ��6J    5�  "   �� 5�  &    62   �� *
IsActive+ True6O   .a�5�  *   �� 	 � �� 
 Active#
 �  6D   5�  '   �� 5�  &    62   �� *
IsActive+	 False6O   .a�5�  *   �� 	 � ��  	NotActive#
 �  6D   5�  '   5�     5�     5�     �� 5�     ��5�     �� 5�     #" [ 6    " ��6L    5�     �� 5�  "    6.   �� *	IsEnabled+1 �� ElementName$1 
SaveButton�� True6d   . 5�  &   �� 5�  *   .��5�  .   �� .��5�  2   �� $1 +(Border.Background).(SolidColorBrush.Color)�5�  5   $/ #9319B8X�5�  5   $ 0:0:0.2D�5�  5   5�  /   5�  +   0 5�  &   �� 5�  *   .��5�  .   �� .��5�  2   �� $1 +(Border.Background).(SolidColorBrush.Color)�5�  5   $/ #3A3D41X�5�  5   $ 0:0:0.2D�5�  5   5�  /   5�  +   5�  #   5�     ._�5�     �� -   5        2 ��Name$2 
SaveButton��5�     $��10q�5�     
������ $��0q�5�     ��5�     �� *SaveOrCreateCommand+��5�     �� 5�     #" [ 6$   " ��6J    5�  "   �� 5�  &    62   �� *IsEdit+ True6M   .a�5�  *   �� 	 � ��  Save#
 �  6D   5�  '   �� 5�  &    62   �� *IsEdit+	 False6M   .a�5�  *   �� 	 � �� 
 Create#
 �  6D   5�  '   5�     5�     5�     �� 5�     #��[ 6   ._�5�     �� $��10q�5�     
������ $��0q�5�       Back#���  5�     #��[ 5�     ��5�     �� *CloseCommand+5�     5�     5�     5�     5�     5�     