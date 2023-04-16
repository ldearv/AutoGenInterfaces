//
//  NetModels.swift
//  Created by AutoIF
//

import Foundation
import HandyJSON

// MARK: 【C1】无参数测试接口1 返回data是字符串
struct Common_noparam1_Return_Model_C1: HandyJSON {
    var code: Int!
    var data: String?
    var msg: String?
}

// MARK: 【C2】无参数测试接口2 返回data是字符串数组
struct Common_noparam2_Return_Model_C2: HandyJSON {
    var code: Int!
    var data: Array<String>?
    var msg: String?
}

// MARK: 【C3】无参数测试接口3 返回data是对象
struct data_Model_C3: HandyJSON {
    var name: String? // 名字
    var age: String? // 年龄
}

struct Common_noparam3_Return_Model_C3: HandyJSON {
    var code: Int!
    var data: data_Model_C3?
    var msg: String?
}

// MARK: 【C4】无参数测试接口4 返回data是对象数组
struct data_Model_C4: HandyJSON {
    var name: String? // 名字
    var age: String? // 年龄
}

struct Common_noparam4_Return_Model_C4: HandyJSON {
    var code: Int!
    var data: Array<data_Model_C4>?
    var msg: String?
}

// MARK: 【C5】有参数测试接1 参数是字符串
struct Common_hasparam1_Return_Model_C5: HandyJSON {
    var code: Int!
    var data: Array<String>?
    var msg: String?
}

// MARK: 【C6】有参数测试接2 参数是字符串数组
struct Common_hasparam2_Return_Model_C6: HandyJSON {
    var code: Int!
    var data: String?
    var msg: String?
}

// MARK: 【C7】有参数测试接3 参数是字典
struct Common_hasparam3_Return_Model_C7: HandyJSON {
    var code: Int!
    var data: String?
    var msg: String?
}

// MARK: 【C8】有参数测试接4 参数是多个字符串
struct Common_hasparam4_Return_Model_C8: HandyJSON {
    var code: Int!
    var data: String?
    var msg: String?
}



