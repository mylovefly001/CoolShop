class Base {

    /**
     * 记录日志
     * @param str
     */
    protected log(str: string = "") {
        let date = new Date();
        console.debug(`${date.toLocaleDateString('chinese', {hour12: false})}:\t${str}`);
    }

    /**
     * 校验字符串是否为空
     * @param str
     */
    public static isEmpty(str: string = "") {
        return str == '' || str == undefined || str.replace(/(^\s*)|(\s*$)/g, "") == "";
    }

    /**
     * 格式化日期
     * @param n
     */
    protected static formatNumber(n) {
        n = n.toString();
        return n[1] ? n : '0' + n
    }
}