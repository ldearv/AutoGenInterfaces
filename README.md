# AutoGenInterfaces
根据配置文件，自动生成网络调用接口


使用方法：
（1） 修改配置文件AutoGenInterfaces/apilist.txt
apilist.txt中列举了几种常见的情况。可以根据需要修改。具体参考apilist里提供的例子，与Demo里生成的结果对比。
（2）执行代码。
如果本机有dotnet 7的环境，可以直接在Program.cs所在的目录用终端执行”dotnet run",结果就会出现在Result目录里。

如果本机没有dotnet 7, 而是有其他dotnet环境，可以用文本工具打开AutoGenInterfaces.csproj，将里面的net7.0改成本机安装的dotnet环境。再执行 dotnet run

另外本程序并不一定要dotnet环境。 如果在Windows上运行，也可以使用.net framework环境。可以新建一个frame项目，然后把本程序文件添加进去。

（3）安装需要的第三方库
本程序提供的Demo中Swift使用的是Moya 和 HandyJSON，可以使用cocoapod添加。
本程序提供的Demo中C#使用的是Newtonsoft.Json.dll，可以使用本程序里的，也可以自己在网络获取。


（4）获取结果文件
dotnet run 执行成功后，会在AutoGenInterfaces/Result下生成一个当前时间的文件夹，里面放的是结果文件，比如MoyaHandyJSON，就是生成的Swift代码，起这个名字是因为这些代码是基于Moya 和 HandyJSON的，把MoyaHandyJSON里的文件拷贝到项目中去，比如SwiftUIDemo/SwiftUIDemo/ConnectAutoIF文件夹里。
C#里的情况类似，也是拷贝生成的文件到自己的项目里。需要注意修改namespace为自己项目的。

（4）获取剩余其他文件
在Demo中提供了网络接口需要的其他文件，这些是随着接口改变，基本不发生改变的部分。
Swift可以使用SwiftUIDemo/SwiftUIDemo/Connect里的。可以拷贝到自己的项目中。
C#可以使用WindowsFormsDemo/NewWork里的；这个要作为一个库添加到自己的解决方案中。把里面的cs文件直接添加到项目中也是可以的。

（5）添加调用的部分。在Demo中提供了简单的调用。


其他说明：
1 接口文档第一个接口前放置">>",最后一个接口后放置">>",接口之间用">>"分隔。
2 每个接口第1行，写接口的名称。不可换行。
3 第2行写接口编号，以“Interface No.”或“接口编号”开头。
4 第3行写接口模块和接口方法，两者以“/”分隔。
5 第4行写发送字段的开始标志。以“POST”或“发送数据”开头。在下一行开始写发送的数据。
6 在写完发送数据后，在下一行写返回状态码的开始标志（“返回状态码”或“ReturnCode”开头）,在下一行开始写返回状态码。不可以没有这个部分。
7 在写完返回状态码后，在下一行写返回数据的开始标志（“返回数据”或“ReturnData”开头），在下一行开始写返回数据。如果没有返回数据，省略这个部分。
8 在返回状态码之后，“>>”之前，不符合返回状态码或返回数据格式的行，会被当做接口备注保存。
9 发送字段和接收字段文档语法：
9.1 字段名用半角中括号和单引号包裹，如['User']
9.2 字段名使用字母和数字，数字不能放在字段开头。
9.3 包含型数据，写成['User']['Name'],代表User包含Name。
9.4 数组型数据，先写一行“{”，在结束的地方再写一行“}”，中间写数组的字段
9.5 字段和字段注释之间用空格（“ ”）或Tab分隔。
10 接口文档apilist.txt要保存为UTF8 with BOM的格式(建议使用sublime text的Save with Encoding).

接口文档字段中间存储方式：
1 顶级字段：
UserID 用户ID
UserName 用户名
2  字典
User*UserID 用户ID
User*UserName 用户名
3 数组(**之间是省略掉的index)
UserList**UserID
4 字典数组
UserList**User*UserID
UserList**User*UserName
5 n维数组（n+1个*，之间的n个位置，代表省略的n个index)



感谢 烟花下的孤独 写下了文章《swift 使用Moya进行网络请求》地址：https://blog.csdn.net/u014651417/article/details/123085545
本程序中Swift部分的接口封装和Demo都是基于这篇文章和其提供的SwiftDemo。

感谢 邵珠勇 提供了C#的Demo。



