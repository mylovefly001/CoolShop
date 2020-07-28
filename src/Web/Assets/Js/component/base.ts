class Base {
    private _debug: boolean = false;

    protected debug(t: boolean = false) {
        this._debug = t;
    }

    /**
     * 校验字符串是否为空
     * @param str
     */
    protected isEmpty(str: string = "") {
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

    protected log(str: string = "") {
        if (this.debug && !this.isEmpty(str)) {
            console.debug(str);
        }
    }
}