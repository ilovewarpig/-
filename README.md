# -
用于arduino教学的数字化支持系统，当前仅能实现识别上传的器件图片

环境：<br/>
visual studio 2017<br/>
.net framework 4.7.2<br/>
sqlserver 2016

第三方库：<br/>
TensorFlowSharp.1.15.1<br/>
NumSharp.0.20.5<br/>
OpenCvSharp

WebApplication页面是初始页面，在该页面上上传图片。<br/>
ShowResults页面显示图像识别的结果，展示可能性最高的三个分类，点击对应图片后直接导航至数字资源

图片识别结果通过Session传递
