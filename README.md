# Mesh

## PolygonImage

正多边形UI。



feature：

- 自定义边数
- 旋转



应用场景：

- 人物能力雷达图
- 地形雷达图



## Procedural Terrain

Feature:

- Quad：多顶点的四边形
- Sector：扇形



## QShadow and QOutline

原生的Shadow和Outline组件在应用之后会将原有的共享顶点拆开，顶点数直接增大为三角形数 * 3，对于一些网格复杂的，共享顶点多的UI在应用Outline之后顶点数增大的情况尤为明显。因为学疏才浅暂时不知道原作者为什么这么写，但还是自己写了另一版Shadow和Outline，使之保留原本的共享顶点。

