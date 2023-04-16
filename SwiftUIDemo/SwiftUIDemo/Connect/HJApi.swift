//
//  HJApi.swift
//  HJNet
//
//  Created by Mr.Heng on 2022/4/26.
//
 
import Moya
import Foundation

extension HJApi: TargetType {

    // MARK: - 域名
    public var baseURL: URL {
        switch self {
        default:
            return URL(string: "http://192.168.1.99/")!
        }
    }
    // MARK: 接口的请求类型
    public var method: Moya.Method {
        switch self {
        default:
            return .post
        }
    }
    
//    public var validate: Bool {
//        return false
//    }
   
    public var validationType: ValidationType {
        return .successCodes
    }
    
    // MARK: - 用于单元测试
    public var sampleData: Data {
        return "{}".data(using: String.Encoding.utf8)!
    }
    public var headers: [String: String]? {
        
        return [:]
    }
}
