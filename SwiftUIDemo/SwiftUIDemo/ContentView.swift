//
//  ContentView.swift
//  SwiftUIDemo
//
//

import SwiftUI


struct ContentView: View {
    
    @StateObject var viewModel: XXViewModel
    
    var body: some View {
        VStack {
            Button {
                viewModel.requestCommonnoparam1()
            } label: {
                Text("无参数测试接口1 返回data是字符串")
            }
            Button {
                viewModel.requestCommonnoparam2()
            } label: {
                Text("无参数测试接口2 返回data是字符串数组")
            }
            Button {
                viewModel.requestCommonnoparam3()
            } label: {
                Text("无参数测试接口3 返回data是对象")
            }
            Button {
                viewModel.requestCommonnoparam4()
            } label: {
                Text("无参数测试接口4 返回data是对象数组")
            }
            Button {
                viewModel.requestCommonhasparam1(param: "xiaoming")
            } label: {
                Text("有参数测试接1 参数是字符串")
            }
            Button {
                viewModel.requestCommonhasparam2(array: ["1", "2", "3"])
            } label: {
                Text("有参数测试接2 参数是字符串数组")
            }
            Button {
                viewModel.requestCommonhasparam3(dic: ["param1" : "1", "param2": "2"])
            } label: {
                Text("有参数测试接3 参数是字典")
            }
            Button {
                viewModel.requestCommonhasparam4(name: "xiaoming", age: "10", city: "kunming")
            } label: {
                Text("有参数测试接4 参数是多个字符串")
            }

        }
        .padding()
    }
}

struct ContentView_Previews: PreviewProvider {
    static var previews: some View {
        ContentView(viewModel: XXViewModel())
    }
}
