//
//  HJApiTargetType.swift
//  Created by AutoIF
//

import Moya
import Foundation

extension HJApi {
    // MARK: - 请求地址
    public var path: String {
        switch self {
        case .Common_noparam1:        //C1 【C1】无参数测试接口1 返回data是字符串
            return "Common/noparam1"
        case .Common_noparam2:        //C2 【C2】无参数测试接口2 返回data是字符串数组
            return "Common/noparam2"
        case .Common_noparam3:        //C3 【C3】无参数测试接口3 返回data是对象
            return "Common/noparam3"
        case .Common_noparam4:        //C4 【C4】无参数测试接口4 返回data是对象数组
            return "Common/noparam4"
        case .Common_hasparam1:        //C5 【C5】有参数测试接1 参数是字符串
            return "Common/hasparam1"
        case .Common_hasparam2:        //C6 【C6】有参数测试接2 参数是字符串数组
            return "Common/hasparam2"
        case .Common_hasparam3:        //C7 【C7】有参数测试接3 参数是字典
            return "Common/hasparam3"
        case .Common_hasparam4:        //C8 【C8】有参数测试接4 参数是多个字符串
            return "Common/hasparam4"
        }
    }

    // MARK: - 请求的参数在这里处理
    public var task: Task {
        switch self {
        case .Common_hasparam1(let parameters):        //C5 【C5】有参数测试接1 参数是字符串
            fallthrough
        case .Common_hasparam2(let parameters):        //C6 【C6】有参数测试接2 参数是字符串数组
            fallthrough
        case .Common_hasparam3(let parameters):        //C7 【C7】有参数测试接3 参数是字典
            fallthrough
        case .Common_hasparam4(let parameters):        //C8 【C8】有参数测试接4 参数是多个字符串
            // MARK: - 增加AppInfo和sign的方式
            let signPostDic = HJNet.getSignPostDataWithDictionary(dicParameters: parameters)
            return .requestParameters(parameters: signPostDic, encoding: JSONEncoding.default)

        default: // 无参数走default
            let signPostDic = HJNet.getSignPostDataWithDictionary(dicParameters: Dictionary.init())
            return .requestParameters(parameters: signPostDic, encoding: JSONEncoding.default)
            
//            //   MARK: - 不增加AppInfo和sign的方式
//            return .requestParameters(parameters: parameters, encoding: JSONEncoding.default)
//
//        default: // 无参数走default
//            return .requestPlain
        }
    }

}
