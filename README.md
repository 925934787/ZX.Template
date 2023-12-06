# .net个人脚手架以及自动发布nuget  

> #### 准备工作
> - 微软账号
> - github账号
> - nuget.exe执行文件

1. 正常创建项目,项目名称中添加可以自定义的占位符，如ZX.Template.Blazor。这里使用的[ZX.Template]作为占位符，后期创建项目，将用实际项目名称代替。  
2.  往项目根目录下添加文件夹.template.config。然后添加文件template.json，内容如下
    ```json
    {
        "$schema": "http://json.schemastore.org/template",
        "author": "作者",
        "classifications": [
            "WPF",
            "Blazor"
        ],
        "identity": "ZX.Template",
        "name": "名字",
        "shortName": "zxwpfblazor(代码创建新项目时候的脚手架缩写)",
        "sourceName": "ZX.Template(用项目名称替换的内容)",
        "tags": {
            "language": "C#",
            "type": "project"
        }
    }
    ```

3. 创建nuget配置文件，后缀为.nuspec
    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <package xmlns="http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd">
    <metadata>
        <id>ZX.Template.WPFBlazor</id><!-- Nuget包名 -->
        <version>1.0.2</version>
        <description>WPF-Blazor</description>
        <authors>作者</authors>
        <packageTypes>
        <packageType name="Template" />
        </packageTypes>
        <icon>icon.png</icon>
        <license type="expression">MIT</license>
    </metadata>
        <files>
        <!-- 文件输出需要按照实际的文件位置修改,排除编译文件，需要包含第二部创建的文件 -->
        <file src=".\**\*" target="content\src\" exclude=".\**\bin\**;.\**\obj\**;.*\**;.\.vs\**" />
        <file src="icon.png"/>
        </files>
    </package>
    ```
4. 使用命令本地生成nuget包，将准备的nuget.exe放到项目根目录下
    ```shell
    .\nuget.exe pack .\ZX.Template.nuspec
    ```
    检查生成的nuget包是否可以用，可以使用微软商店的**Nuget Package Explorer**查看nuget包具体内容。

5. 微软账号登录nuget网站，通过账号里面的API keys生成key，到这里也可以手动上传nuget包。

6. 代码上传github，创建.net的action。
    ~~~yaml
    name: .NET

    on:
    push:
        #此处为触发器，这里使用tag触发，如果打了v开头的tag，就自动生成nuget包并发布
        tags:
        - v*

    jobs:
    build:

        runs-on: windows-latest

        steps:
        - uses: actions/checkout@v3
        - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
            dotnet-version: 8.0.x
        - name: Build
        run: |
        #打包nuget，使用自己实际命名的nuspec文件
            .\nuget pack .\ZX.Template.nuspec
        # 推送到nuget
        - name: Push generated package to nuget
        run: |
            .\nuget push .\*.nupkg -Source https://api.nuget.org/v3/index.json -SkipDuplicate -ApiKey ${{ secrets.NugetKey }} -NoSymbols
    ~~~

7. github创建secrets.NugetKey，在项目的settings->secrets->actions,中的Repository secrets里面创建名为NugetKey的密钥，内容就是5处生成的key
