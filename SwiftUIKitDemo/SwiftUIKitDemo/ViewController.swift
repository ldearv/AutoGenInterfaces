//
//  ViewController.swift
//  SwiftUIKitDemo
//

import UIKit

class ViewController: UIViewController {

    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view.
    }

    @IBOutlet weak var outLabel: UILabel!
    @IBAction func touchBtn(_ sender: UIButton) {
        requestCommonnoparam1()        // C1 【C1】无参数测试接口1 返回data是字符串
        requestCommonnoparam2()        // C2 【C2】无参数测试接口2 返回data是字符串数组
        requestCommonnoparam3()        // C3 【C3】无参数测试接口3 返回data是对象
        requestCommonnoparam4()        // C4 【C4】无参数测试接口4 返回data是对象数组
        requestCommonhasparam1(param: "xiaoming")        // C5 【C5】有参数测试接1 参数是字符串
        requestCommonhasparam2(array: ["1", "2", "3"])     // C6 【C6】有参数测试接2 参数是字符串数组
        requestCommonhasparam3(dic: ["param1" : "1", "param2": "2"])     // C7 【C7】有参数测试接3 参数是字典
        requestCommonhasparam4(name: "xiaoming", age: "10", city: "kunming")       // C8 【C8】有参数测试接4 参数是多个字符串
    }
    var data_c1: String = ""
    var data_c2: Array<String> = []
    var data_c3: data_Model_C3? = nil
    var data_c4: Array<data_Model_C4> = []
    var data_c5: Array<String>? = nil
    var data_c6: String? = nil
    var data_c7: String? = nil
    var data_c8: String? = nil
}

// MARK: - 网络封装
extension ViewController {
    // MARK: 【C1】无参数测试接口1 返回data是字符串
    func requestCommonnoparam1() {
        HJNet.request(target: HJApi.Common_noparam1) { code, result in
#if APIDebug1
            print(#function)
            print("code:\(code)")
            print("result:\(result)")
#endif
            if let tempModel = Common_noparam1_Return_Model_C1.deserialize(from: result) {
#if APIDebug1
                print(#function)
                HJNet.compareJson(preJson: result, nextJson: (tempModel.toJSONString() ?? "{}"))
#endif
                if tempModel.code == 200 {
                    if let data = tempModel.data {
                        self.data_c1 = data
                        self.outLabel.text = data
                    }
                } else {
                    print("code 不等于 200")
                }
            }
        } failureBlock: { code, msg in
            print(#function)
            print("联网失败：code->\(code), msg->\(msg)")
        }
    }

    // MARK: 【C2】无参数测试接口2 返回data是字符串数组
    func requestCommonnoparam2() {
        HJNet.request(target: HJApi.Common_noparam2) { code, result in
#if APIDebug1
            print(#function)
            print("code:\(code)")
            print("result:\(result)")
#endif
            if let tempModel = Common_noparam2_Return_Model_C2.deserialize(from: result) {
#if APIDebug1
                print(#function)
                HJNet.compareJson(preJson: result, nextJson: (tempModel.toJSONString() ?? "{}"))
#endif
                if tempModel.code == 200 {
                    if let data = tempModel.data {
                        self.data_c2 = data
                    }
                } else {
                    print("code 不等于 200")
                }
            }
        } failureBlock: { code, msg in
            print(#function)
            print("联网失败：code->\(code), msg->\(msg)")
        }
    }

    // MARK: 【C3】无参数测试接口3 返回data是对象
    func requestCommonnoparam3() {
        HJNet.request(target: HJApi.Common_noparam3) { code, result in
#if APIDebug1
            print(#function)
            print("code:\(code)")
            print("result:\(result)")
#endif
            if let tempModel = Common_noparam3_Return_Model_C3.deserialize(from: result) {
#if APIDebug1
                print(#function)
                HJNet.compareJson(preJson: result, nextJson: (tempModel.toJSONString() ?? "{}"))
#endif
                if tempModel.code == 200 {
                    if let data = tempModel.data {
                        self.data_c3 = data
                    }
                } else {
                    print("code 不等于 200")
                }
            }
        } failureBlock: { code, msg in
            print(#function)
            print("联网失败：code->\(code), msg->\(msg)")
        }
    }

    // MARK: 【C4】无参数测试接口4 返回data是对象数组
    func requestCommonnoparam4() {
        HJNet.request(target: HJApi.Common_noparam4) { code, result in
#if APIDebug1
            print(#function)
            print("code:\(code)")
            print("result:\(result)")
#endif
            if let tempModel = Common_noparam4_Return_Model_C4.deserialize(from: result) {
#if APIDebug1
                print(#function)
                HJNet.compareJson(preJson: result, nextJson: (tempModel.toJSONString() ?? "{}"))
#endif
                if tempModel.code == 200 {
                    if let data = tempModel.data {
                        self.data_c4 = data
                    }
                } else {
                    print("code 不等于 200")
                }
            }
        } failureBlock: { code, msg in
            print(#function)
            print("联网失败：code->\(code), msg->\(msg)")
        }
    }

    // MARK: 【C5】有参数测试接1 参数是字符串
    func requestCommonhasparam1(param: String) {
        let param = ["param": param] as [String : Any]
        HJNet.request(target: HJApi.Common_hasparam1(parameters: param)) { code, result in
#if APIDebug1
            print(#function)
            print("code:\(code)")
            print("result:\(result)")
#endif
            if let tempModel = Common_hasparam1_Return_Model_C5.deserialize(from: result) {
#if APIDebug1
                print(#function)
                HJNet.compareJson(preJson: result, nextJson: (tempModel.toJSONString() ?? "{}"))
#endif
                if tempModel.code == 200 {
                    if let data = tempModel.data {
                        self.data_c5 = data
                    }
                } else {
                    print("code 不等于 200")
                }
            }
        } failureBlock: { code, msg in
            print(#function)
            print("联网失败：code->\(code), msg->\(msg)")
        }
    }

    // MARK: 【C6】有参数测试接2 参数是字符串数组
    func requestCommonhasparam2(array: [String]) {
        let param = ["params": array] as [String : Any]
        HJNet.request(target: HJApi.Common_hasparam2(parameters: param)) { code, result in
#if APIDebug1
            print(#function)
            print("code:\(code)")
            print("result:\(result)")
#endif
            if let tempModel = Common_hasparam2_Return_Model_C6.deserialize(from: result) {
#if APIDebug1
                print(#function)
                HJNet.compareJson(preJson: result, nextJson: (tempModel.toJSONString() ?? "{}"))
#endif
                if tempModel.code == 200 {
                    if let data = tempModel.data {
                        self.data_c6 = data
                    }
                } else {
                    print("code 不等于 200")
                }
            }
        } failureBlock: { code, msg in
            print(#function)
            print("联网失败：code->\(code), msg->\(msg)")
        }
    }

    // MARK: 【C7】有参数测试接3 参数是字典
    func requestCommonhasparam3(dic: [String: String]) {
        //let param = ["keyword": ["param1": "param1", "param2": "param2"]] as [String : Any]
        let param = ["keyword": dic] as [String: Any]
        HJNet.request(target: HJApi.Common_hasparam3(parameters: param)) { code, result in
#if APIDebug1
            print(#function)
            print("code:\(code)")
            print("result:\(result)")
#endif
            if let tempModel = Common_hasparam3_Return_Model_C7.deserialize(from: result) {
#if APIDebug1
                print(#function)
                HJNet.compareJson(preJson: result, nextJson: (tempModel.toJSONString() ?? "{}"))
#endif
                if tempModel.code == 200 {
                    if let data = tempModel.data {
                        self.data_c7 = data
                    }
                } else {
                    print("code 不等于 200")
                }
            }
        } failureBlock: { code, msg in
            print(#function)
            print("联网失败：code->\(code), msg->\(msg)")
        }
    }

    // MARK: 【C8】有参数测试接4 参数是多个字符串
    func requestCommonhasparam4(name: String, age: String, city: String) {
        let param = ["name": name, "age": age, "city": city] as [String : Any]
        HJNet.request(target: HJApi.Common_hasparam4(parameters: param)) { code, result in
#if APIDebug1
            print(#function)
            print("code:\(code)")
            print("result:\(result)")
#endif
            if let tempModel = Common_hasparam4_Return_Model_C8.deserialize(from: result) {
#if APIDebug1
                print(#function)
                HJNet.compareJson(preJson: result, nextJson: (tempModel.toJSONString() ?? "{}"))
#endif
                if tempModel.code == 200 {
                    if let data = tempModel.data {
                        self.data_c8 = data
                    }
                    
                } else {
                    print("code 不等于 200")
                }
            }
        } failureBlock: { code, msg in
            print(#function)
            print("联网失败：code->\(code), msg->\(msg)")
        }
    }

}

