//
//  HJNet.swift
//  HJNet
//
//  Created by Mr.Heng on 2022/4/26.
//
 
import Moya
import Alamofire

import CommonCrypto

enum HTTPResultType: Int {
    case failure    //请求失败
    case success    //请求成功
}

/// 网络请求发送的核心初始化方法，创建网络请求对象
private let provider = MoyaProvider<MultiTarget>(endpointClosure: endpointClosure, requestClosure: requestClosure, plugins: [networkPlugin], trackInflights: false)
///超时时长
private var requestTimeOut: Double = 30
/// 网络请求的基本设置,这里可以拿到是具体的哪个网络请求，可以在这里做一些设置
private let endpointClosure = {(target: TargetType) -> Endpoint in
   let url = target.baseURL.absoluteString + target.path
   var task = target.task
   var endPoint = Endpoint(url: url,
                           sampleResponseClosure: { .networkResponse(200, target.sampleData) },
                           method: target.method,
                           task: task,
                           httpHeaderFields: target.headers)
   if let apiTarget = target as? MultiTarget,
      let tar = apiTarget.target as? HJApi  {
       switch tar {
       default:
           return endPoint
       }
   }
   return endPoint
}
/// 网络请求的设置
private let requestClosure = { (endpoint: Endpoint, done: MoyaProvider.RequestResultClosure) in
   do {
       var request = try endpoint.urlRequest()
       // 设置请求时长
       request.timeoutInterval = requestTimeOut
       // 打印请求参数
       if let requestData = request.httpBody {
           print("请求的url：\(request.url!)" + "\n" + "\(request.httpMethod ?? "")" + "发送参数" + "\(String(data: request.httpBody!, encoding: String.Encoding.utf8) ?? "")")
       } else {
           print("请求的url：\(request.url!)" + "\(String(describing: request.httpMethod))")
       } 
//       if let header = request.allHTTPHeaderFields {
//           print("请求头内容\(header)")
//       }

       done(.success(request))
   } catch {
       done(.failure(MoyaError.underlying(error, nil)))
   }
}

private let networkPlugin = NetworkActivityPlugin.init { changeType, _ in
   //print("networkPlugin \(changeType)")
   // targetType 是当前请求的基本信息
   switch changeType {
   case .began:
       print("开始请求网络\n\n\n")

   case .ended:
       print("\n\n结束请求网络")
   }
}

public class HJNet {
   private static let msgNetError = "网络错误，请联网后点击重试"

   ///返回字符串 网络请求
   public class func request(target: TargetType,
                       successBlock: @escaping (_ code: String, _ result: String) -> Void,
                       failureBlock:@escaping (_ code: String, _ msg:String) -> Void) {
       
       provider.request(MultiTarget(target)) { result in
           switch result {
           case let .success(response):
               if let strData = String(data:response.data, encoding: .utf8) {
                   successBlock("200", strData)
               } else {
                   failureBlock("-1", "response.data 转字符串失败")
               }
           case let .failure(error):
               print("failure\(error.localizedDescription)")
               return failureBlock("-1", msgNetError)
           }
       }
   }
    
    public class func getSignPostDataWithDictionary(dicParameters: Dictionary<String, Any>) -> Dictionary<String, Any> {
        // MARK: 下面4行是获取APP的一些信息，一般是服务器有统计用途，或根据手机系统的语言返回相应语言的文字用；还有一个用处是在接口参数为空时，避免了post数据为空的情况。 如果确实不需要参数，可以修改HJApiTaskType,最后的几行代码为不增加AppInfo和sign的方式。
        var postDictionary: Dictionary<String, Any> = self.getAppInfoDictionary()
        if !dicParameters.isEmpty {
            postDictionary += dicParameters
        }
        // TODO: 下面注释的两行是对post数据求md5,用来在服务器端验证是否是正常请求使用的。计算方式需要前后端一致才可以。
        //let strSign: String = self.getSignWithDictionary(dicData: postDictionary)
        //postDictionary.updateValue(strSign.md5, forKey: "sign")
        return postDictionary
    }
    
    
    public class func getAppInfoDictionary() -> Dictionary<String, Any> {
        let userDefaults = UserDefaults.standard
        var strLang = "1"
        let languages: Array<String> = userDefaults.object(forKey: "AppleLanguages") as! Array<String>
        let preferredlang = languages[0] as String
        if preferredlang == "zh-Hans-CN"
            || preferredlang == "zh-Hant-CN"
            || preferredlang == "zh-Hans-US"
            || preferredlang == "zh-TW"
            || preferredlang == "zh-HK"
            || preferredlang == "zh-Hans" {
            strLang = "2"
        }
        
        // 获取APP信息
        let infoDictionary: Dictionary = Bundle.main.infoDictionary!
        // APP当前版本号
        let appVersion = infoDictionary["CFBundleShortVersionString"] as! String
        
        // APP名称
        //let kAppDisplayName = infoDictionary["CFBundleDisplayName"] as! String
        
        // build号
        //let kAppMinorVersion = infoDictionary["CFBundleVersion"] as! String
        
        // bundleId
        //let kBundleId: String = Bundle.main.bundleIdentifier ?? ""
        
        // 系统名称
        //let osName: String = UIDevice.current.systemName
        
        // 系统版本， 如 14.6
        //let osVersion: String = UIDevice.current.systemVersion
        
        // 设备UUID
        let uuid = UIDevice.current.identifierForVendor!.uuidString
        
        // 设备型号, 如iPhone
        //let deviceType: String = UIDevice.current.model
        
        let dicInfo = [
            "uuid":uuid,
            "version": appVersion,
            "lang_id":strLang
        ]
        return dicInfo
    }
    
    public class func getSignWithDictionary(dicData: Dictionary<String, Any>) -> String
    {
        // TODO: ”salt“ 更换为自己的”盐“，加盐方式也可以不使用在字符串后追加，而是改成自己定义的方式
        return dic2Str(dicData: dicData) + "salt"
    }
    
    private class func dic2Str(dicData: Dictionary<String, Any>) -> String {
        // 字典转换成字符串
        //let sortDescriptor = NSSortDescriptor(key:nil, ascending: true)
        //let keys = dicData.keys
        let sortDic = dicData.sorted(by: {$0.0 < $1.0})
        var strSign: String = ""
        for item in sortDic {
            strSign = strSign + item.key
            strSign = strSign + "="
            
            var value: String = ""
            if item.value is Array<Any> {
                let data = try? JSONSerialization.data(withJSONObject: item.value, options: [])
                value = String(data: data ?? Data.init(), encoding: String.Encoding.utf8) ?? ""
            } else if item.value is Dictionary<String, Any> {
                value = dic2Str(dicData: item.value as! Dictionary<String, Any>)
            }
            else {
                value = item.value as! String
            }
            
            strSign = strSign + value
            
            strSign = strSign + "&"
        }
        let strSign_tmp = strSign.dropLast(1)
        return String(strSign_tmp)
    }
    
    static func compareJson(preJson: String, nextJson: String) {
        let preData: Data = preJson.data(using: .utf8)!
        let nextData: Data = nextJson.data(using: .utf8)!
        
        let preDict = try? JSONSerialization.jsonObject(with: preData, options: .mutableContainers)
        let nextDict = try? JSONSerialization.jsonObject(with: nextData, options: .mutableContainers)
        
        if preDict != nil && nextDict != nil {
            let result = resurceComJson(pre: preDict!, next: nextDict!)
            if result != nil {
                if (!JSONSerialization.isValidJSONObject(result!)) {
                    print("无法解析出JSONString")
                }
                let data : NSData! = try? JSONSerialization.data(withJSONObject: result!, options: []) as NSData?
                let JSONString = NSString(data:data as Data,encoding: String.Encoding.utf8.rawValue)
                //print("result: \(preJson)")
                print("Json差别: \(JSONString ?? "转json失败")")
            } else {
                print("Json一致")
            }
        } else {
            print("解析失败")
        }
    }
    
    static private func resurceComJson(pre: Any, next: Any) -> Any? {

        if pre is [String: Any] {
            // 字典
            var resultDic = [String: Any]()
            for (keyPre, valuePre) in (pre as! [String: Any]) {
                //print("\(keyPre) is \(valuePre)")
                if let nextHasKey = (next as? [String: Any])?.contains(where: { (key: String, value: Any) in key == keyPre }), nextHasKey == true {
                    let valueCom = resurceComJson(pre: valuePre, next: (next as! [String: Any])[keyPre] ?? "")
                    if valueCom != nil {
                        resultDic[keyPre] = valueCom
                    }
                } else {
                    if valuePre is NSNull {} else {
                        resultDic[keyPre] = valuePre
                    }
                }
            }
            if resultDic.isEmpty {
                return nil
            }
            return resultDic
        } else if pre is [Any] {
            // 数组
            var resultArray = [Any]()
            for (index, value) in (pre as! [Any]).enumerated() {
                if index < (next as! [Any]).count {
                    let valueCom = resurceComJson(pre: value, next: (next as! [Any])[index])
                    if valueCom != nil {
                        resultArray.append(valueCom!)
                    }
                } else {
                    resultArray.append(value)
                }
            }
            if resultArray.isEmpty {
                return nil
            }
            return resultArray
        } else {
            // 叶子节点
            if String("\(pre)") == String("\(next)") {
                return nil
            } else {
                return pre
            }
        }
    }
}

func += <KeyType, ValueType> ( left: inout Dictionary<KeyType, ValueType>, right: Dictionary<KeyType, ValueType>) {
    for (k, v) in right {
        left.updateValue(v, forKey: k)
    }
}

extension String {
    /// 原生md5
    public var md5: String {
        guard let data = data(using: .utf8) else {
            return self
        }
        var digest = [UInt8](repeating: 0, count: Int(CC_MD5_DIGEST_LENGTH))
        _ = data.withUnsafeBytes { (bytes: UnsafeRawBufferPointer) in
            return CC_MD5(bytes.baseAddress, CC_LONG(data.count), &digest)
        }
        return digest.map { String(format: "%02x", $0) }.joined()

    }
}
