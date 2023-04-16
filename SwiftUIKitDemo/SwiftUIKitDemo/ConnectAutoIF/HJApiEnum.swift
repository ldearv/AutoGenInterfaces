//
// Created by AutoIF
//

public enum HJApi {
    case Common_noparam1        // MARK: 【C1】无参数测试接口1 返回data是字符串
    case Common_noparam2        // MARK: 【C2】无参数测试接口2 返回data是字符串数组
    case Common_noparam3        // MARK: 【C3】无参数测试接口3 返回data是对象
    case Common_noparam4        // MARK: 【C4】无参数测试接口4 返回data是对象数组
    case Common_hasparam1(parameters: Dictionary<String, Any>)        // MARK: 【C5】有参数测试接1 参数是字符串
    case Common_hasparam2(parameters: Dictionary<String, Any>)        // MARK: 【C6】有参数测试接2 参数是字符串数组
    case Common_hasparam3(parameters: Dictionary<String, Any>)        // MARK: 【C7】有参数测试接3 参数是字典
    case Common_hasparam4(parameters: Dictionary<String, Any>)        // MARK: 【C8】有参数测试接4 参数是多个字符串
}
