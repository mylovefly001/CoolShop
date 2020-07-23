class Base {
    /**
     * 校验字符串是否为空
     * @param str
     */
    protected static isEmpty(str: string = "") {
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