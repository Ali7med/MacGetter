   M S B A M L   `   `   `  ����E  AMacGetter2, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null'clr-namespace:MacGetter2
MacGetter2  5   	       MacGetter2.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:MacGetter2     5   	     � Title$  
MainWindow��5      $	��500��6   $	��800��6(   .��5	      �  J� RenderTransformOrigin$ 
0.45,0.504&�6    � RowDefinitions 5
   
   �� 5      $
��217* �6   �� 5      $	��25* �6    � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   �� -   5        $��	BtnGetMac��5      $��	120,0,0,0q�6:   ���	#FFFFA5A5$	��125��6~   $��1��6�   $
��Left=�6�   $��30��6�   $	��Top=�6�   .��6�   	Get Mac6�   �� 5   
   -   5        $��BtnSave��5      $��	250,0,0,0q�66   ���	#FFA5E8FF$	��100��6`   $��1��6l   $
��Left=�6y   $��30��6�   $	��Top=�6�   .��6�   Save6�   �� 5   
   -   5         ��Name$ txtMac��5      $��25��6"   $��
120,0,0,23q�6.   $��Bottom=�6B   $	��230��6]   $
��Left=�6�   �� 5   
   -   5        $ txtIp��5      $��25��6!   $��
120,0,0,63q�6-   $��Bottom=�6A   $	��230��6\   $
��Left=�6�   �� 5   
   -   5         ��Name$ labelMac��5      $��Mac Address��6"   $��	30,0,0,20q�68   $ 0.471,0.002&�6K   $��30��6o   $��Bottom=�6{   $��90��6�   $
��Left=�6�   �� 5   
   -   5        $ labelIp��5      $��
IP Address��6!   $��	30,0,0,60q�66   $ 0.471,0.002&�6I   $��30��6m   $��Bottom=�6y   $��90��6�   $
��Left=�6�   �� 5   
   -   5        $��MenList��5   (   $	��414��5      $��
10,0,10,10q�6"   $��Bottom=�5   
   $��1��69    �� View 5      �� 5      .P�5      �� $��ID��6%    �� Width$ 20��5       �� DisplayMemberBinding 5   3   �� *ID+5      �� 5!      $��IP��6%   $	 150��5"       5!   3   �� *IP+5"      �� 5$      $	��Mac��6%   $	 150��5%       5$   4   �� *Mac+5%      5'      5*      �� 5+   
   -   5        	 ��Name$	 BtnGetMacNetwork��5+      $��10,0,0,0q�6J   ���	#FFFFA5A5$	��125��6s   $��1��6   $
��Left=�6�   $��30��6�   $	��Top=�6�   $��Get Mac��6�   $��1��6  5,      5-      