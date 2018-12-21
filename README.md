# Flashing_Light
这是一个贪食蛇操作模式的RougueLike游戏，操作为上下左右转向，z射击，空格可以放置测试用光源  
由于背景是光与黑暗的斗争，专门编写了2D光照渲染与实时阴影系统。  

其中光照部分包括图集的bump map的自动生成，使用matlab实现，源码整理中  
实时阴影使用CS识别生成，其实是比较低效的办法   
  
原本计划为主角添加动态纹理，并实现  
[Real-Time Polygonal-Light Shading with Linearly Transformed Cosines]https://eheitzresearch.wordpress.com/415-2/  
渲染动态光源，无奈时间有限作罢  
之后有时间考虑将bump map的生成移植到UnityEditor中
