var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Base = /** @class */ (function () {
    function Base() {
    }
    /**
     * 记录日志
     * @param str
     */
    Base.prototype.log = function (str) {
        if (str === void 0) { str = ""; }
        var date = new Date();
        console.debug(date.toLocaleDateString('chinese', { hour12: false }) + ":\t" + str);
    };
    /**
     * 校验字符串是否为空
     * @param str
     */
    Base.isEmpty = function (str) {
        if (str === void 0) { str = ""; }
        return str == '' || str == undefined || str.replace(/(^\s*)|(\s*$)/g, "") == "";
    };
    /**
     * 格式化日期
     * @param n
     */
    Base.formatNumber = function (n) {
        n = n.toString();
        return n[1] ? n : '0' + n;
    };
    return Base;
}());
var Form = /** @class */ (function (_super) {
    __extends(Form, _super);
    function Form() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    /**
     * iframe重设大小
     * @param layer
     */
    Form.iframeResize = function (layer) {
        var _this = this;
        var i = 0;
        this.isAuto = setInterval(function () {
            layer.iframeAuto(layer.getFrameIndex(window.name));
            i++;
            if (typeof (_this.isAuto) === "number" && i == 10) {
                clearInterval(_this.isAuto);
                _this.isAuto = null;
            }
        }, 100);
    };
    Form.Dialog = function (param) {
        if (param === void 0) { param = {}; }
        if (!param.hasOwnProperty("form")) {
            return false;
        }
        if (!param.hasOwnProperty("layer")) {
            return false;
        }
        if (!param.hasOwnProperty("ajaxUrl")) {
            return false;
        }
        var that = this;
        $(param["form"]).form({
            fields: param.hasOwnProperty("fields") ? param["fields"] : null,
            inline: param.hasOwnProperty("inline") ? param["inline"] : true,
            on: "change",
            onSuccess: function (event, fields) {
                var loading = param["layer"].load(2);
                if (param.hasOwnProperty("before") && typeof (param["before"]) === "function") {
                    fields = param["before"](fields);
                }
                $.ajax({
                    url: param['ajaxUrl'],
                    type: "POST",
                    data: fields,
                    dataType: "JSON",
                    success: function (rs) {
                        if (param.hasOwnProperty("success") && typeof (param["success"]) === "function") {
                            param["success"](rs["data"]);
                        }
                    },
                    error: function (request) {
                        if (Base.isEmpty(request.responseText)) {
                            param["layer"].msg(request.statusText, { icon: 2 });
                        }
                        else {
                            param["layer"].msg(request.responseJSON.msg, { icon: 2 });
                        }
                    },
                    complete: function (request) {
                        param["layer"].close(loading);
                    }
                });
            },
            onValid: function () {
                if (param.hasOwnProperty("autoIframe") && param["autoIframe"] == true) {
                    that.iframeResize(param["layer"]);
                }
            },
            onInvalid: function () {
                if (param.hasOwnProperty("autoIframe") && param["autoIframe"] == true) {
                    that.iframeResize(param["layer"]);
                }
            }
        });
    };
    Form.Sidebar = function (param) {
        if (param === void 0) { param = {}; }
        if (!param.hasOwnProperty("form")) {
            return false;
        }
        if (!param.hasOwnProperty("layer")) {
            return false;
        }
        if (!param.hasOwnProperty("ajaxUrl")) {
            return false;
        }
        $(param["form"]).form({
            fields: param.hasOwnProperty("fields") ? param["fields"] : null,
            inline: param.hasOwnProperty("inline") ? param["inline"] : true,
            on: "change",
            onSuccess: function (event, fields) {
                $(event.target).addClass("loading");
                if (param.hasOwnProperty("before") && typeof (param["before"]) === "function") {
                    fields = param["before"](fields);
                }
                $.ajax({
                    url: param['ajaxUrl'],
                    type: "POST",
                    data: fields,
                    dataType: "JSON",
                    success: function (rs) {
                        if (param.hasOwnProperty("success") && typeof (param["success"]) === "function") {
                            param["success"](rs["data"]);
                        }
                    },
                    error: function (request) {
                        if (Base.isEmpty(request.responseText)) {
                            param["layer"].msg(request.statusText, { icon: 2 });
                        }
                        else {
                            param["layer"].msg(request.responseJSON.msg, { icon: 2 });
                        }
                    },
                    complete: function (request) {
                        $(event.target)
                            .removeClass("loading")
                            .closest(".ui.right.sidebar")
                            .sidebar("toggle");
                    }
                });
            }
        });
    };
    Form.isAuto = null;
    return Form;
}(Base));
var MenuTree = /** @class */ (function () {
    function MenuTree() {
    }
    return MenuTree;
}());
var Menu = /** @class */ (function (_super) {
    __extends(Menu, _super);
    function Menu(param) {
        if (param === void 0) { param = {}; }
        var _this = _super.call(this) || this;
        /**
         * 是否显示子节点，true=显示，false=隐藏
         */
        _this.showChild = true;
        /**
         * 勾选类型，1=单选框，2=多选框
         */
        _this.checkType = 1;
        /**
         * 节点事件
         */
        _this.nodeEvent = null;
        if (!param.hasOwnProperty("container")) {
            _super.prototype.log.call(_this, "参数：container 未设置");
            return _this;
        }
        _this.container = param["container"];
        if (!param.hasOwnProperty("layer")) {
            _super.prototype.log.call(_this, "参数：layer 未设置");
            return _this;
        }
        _this.layer = param["layer"];
        if (!param.hasOwnProperty("childData") && !param.hasOwnProperty("ajax")) {
            _super.prototype.log.call(_this, "参数：childData||ajax 未设置");
            return _this;
        }
        if (param.hasOwnProperty("checkType") && param["checkType"] == 2) {
            _this.checkType = param["checkType"];
        }
        if (param.hasOwnProperty("nodeEvent") && typeof (param['nodeEvent']) == "function") {
            _this.nodeEvent = param["nodeEvent"];
        }
        if (param.hasOwnProperty("childData")) {
            _this.childData = param["childData"];
            _this.init();
        }
        else {
            var ajaxParam = param["ajax"];
            _this.container.addClass("loading");
            $.ajax({
                url: ajaxParam["url"],
                type: ajaxParam["type"],
                dataType: ajaxParam["dataType"],
                success: function (rs) {
                    _this.childData = rs["data"];
                    _this.init();
                },
                error: function (request) {
                    if (Base.isEmpty(request.responseText)) {
                        _this.layer.msg(request.statusText, { icon: 2 });
                    }
                    else {
                        _this.layer.msg(request.responseJSON.msg, { icon: 2 });
                    }
                },
                complete: function (request) {
                    _this.container.removeClass("loading");
                }
            });
        }
        return _this;
    }
    Menu.getClassName = function (idx, data) {
        if (idx === void 0) { idx = 0; }
        if (data === void 0) { data = {}; }
        var name = "";
        if (data.length == 1) {
            name += "last";
        }
        else {
            if (idx == 0) {
                name += "first";
            }
            else if (idx == data.length - 1) {
                name += "last";
            }
            else {
                name += "normal";
            }
        }
        return name;
    };
    Menu.prototype.getNodeHtml = function (dataInfo, hasChild, className) {
        if (hasChild === void 0) { hasChild = true; }
        if (className === void 0) { className = ""; }
        var html = "<div class=\"" + className + " node\" bind-id=\"" + dataInfo.id + "\">";
        html += "<span class=\"separate\"></span>";
        //如果是目录
        if (dataInfo.type == 1) {
            html += "<i style=\"line-height: 15px; cursor: pointer;\" class=\"icon folder " + ((this.showChild && hasChild) ? "open" : "") + " outline\" bind-tag=\"folder\"></i>";
        }
        else {
            html += "<i style=\"line-height: 15px;\" class=\"icon file alternate outline\"></i>";
        }
        html += "<div class=\"ui " + (this.checkType == 1 ? "radio" : "") + " checkbox\" style=\"width: 20px;\">\n                        <input type=\"" + (this.checkType == 1 ? "radio" : "checkbox") + "\" id=\"checkbox_" + dataInfo.id + "\" name=\"treeCheckBox\">\n                    </div>";
        //如果是目录，并且有图标
        if (dataInfo.type == 1 && !Base.isEmpty(dataInfo.icon)) {
            html += "<i style=\"line-height: 15px;\" class=\"icon " + dataInfo.icon + "\"></i>";
        }
        if (dataInfo.status == 1) {
            html += "<label>" + dataInfo.text + "</label>";
        }
        else {
            html += "<label class=\"disabled\">" + dataInfo.text + "</label>";
        }
        html += "<span style=\"margin: 0 5px; color: #DEDEDE;\">[" + dataInfo.child.length + "]</span>";
        html += "</div>";
        return html;
    };
    Menu.prototype.getNode = function (dataList) {
        if (dataList === void 0) { dataList = []; }
        var html = "";
        if (dataList.length <= 0) {
            return html;
        }
        for (var i = 0; i < dataList.length; i++) {
            var hasChild = dataList[i].hasOwnProperty("child") && dataList[i].child.length > 0;
            var className = Menu.getClassName(i, dataList);
            html += this.getNodeHtml(dataList[i], hasChild, className);
            if (hasChild) {
                html += "<div class=\"" + className + " child\" style=\"" + (this.showChild ? "" : "display:none;") + "\">" + this.getNode(dataList[i].child) + "</div>";
            }
        }
        return html;
    };
    Menu.prototype.init = function () {
        var that = this;
        var html = this.getNode(this.childData);
        this.container.html(html);
        //当选择/取消选择勾选框时
        this.container.find(".ui.checkbox").checkbox({
            fireOnInit: true,
            onChange: function () {
                if (that.checkType == 2) {
                    $.each($(this).parents(".child"), function (idx, child) {
                        var num = 0;
                        var childObj = $(child).find(".ui.checkbox");
                        $.each(childObj, function (i, item) {
                            if ($(item).checkbox("is checked")) {
                                num += 1;
                            }
                        });
                        var parentObj = $(child).prev(".node").find(".ui.checkbox");
                        //如果全部未选中，则是未选中状态
                        if (num == 0) {
                            parentObj.checkbox("set unchecked");
                        }
                        else {
                            //如果部分选中，则是半掩状态
                            if (num < childObj.length) {
                                parentObj.checkbox("set indeterminate");
                            }
                            else {
                                parentObj.checkbox("set checked");
                            }
                        }
                    });
                }
            },
            onChecked: function () {
                if (that.checkType == 2) {
                    var obj = $(this).parents(".node").next(".child").find(".ui.checkbox");
                    obj.checkbox("set checked");
                }
            },
            onUnchecked: function () {
                if (that.checkType == 2) {
                    var obj = $(this).parents(".node").next(".child").find(".ui.checkbox");
                    obj.checkbox("set unchecked");
                }
            }
        });
        //更改展开/关闭时的图标
        this.container.find("i[bind-tag='folder']").on("click", function () {
            var t = this;
            $(this).closest(".node").next(".child").toggle('fast', function () {
                $(this).css("display") == "block" ? $(t).addClass("open") : $(t).removeClass("open");
            });
        });
        //移入/移出文字的时候颜色改变
        this.container.find(".node>label").mouseenter(function () {
            var color = $(this).css("color");
            $(this).css({ "color": "#1678c2" }).attr("old-color", color);
        }).mouseleave(function () {
            var color = $(this).attr("old-color");
            $(this).css({ "color": color });
        });
        //如果有点击事件
        if (this.nodeEvent != null && typeof (this.nodeEvent) == "function") {
            this.container.find(".node>label").on("click", function () {
                that.nodeEvent(this);
            });
        }
    };
    /**
     * 收缩动作
     * @param isExpand
     */
    Menu.prototype.expand = function (isExpand) {
        if (isExpand === void 0) { isExpand = true; }
        //更改展开/关闭时的图标
        if (isExpand) {
            this.container.find(".child").show("fast", function () {
                $(this).prev(".node").children("i").addClass("open");
            });
        }
        else {
            this.container.find(".child").hide("fast", function () {
                $(this).prev(".node").children("i").removeClass("open");
            });
        }
    };
    /**
     * 全部选择或者全部取消
     * @param isSelect
     */
    Menu.prototype.select = function (isSelect) {
        if (isSelect === void 0) { isSelect = true; }
        if (isSelect) {
            this.container.find(".ui.checkbox").checkbox("set checked");
        }
        else {
            this.container.find(".ui.checkbox").checkbox("set unchecked");
        }
    };
    /**
     * 获取选择的
     */
    Menu.prototype.getSelectVal = function () {
        var ids = [];
        this.container.find(".ui.checkbox").each(function () {
            if ($(this).checkbox("is checked")) {
                var id = parseInt($(this).parent(".node").attr("bind-id"));
                ids.push(id);
            }
        });
        return ids;
    };
    return Menu;
}(Base));
var Table = /** @class */ (function (_super) {
    __extends(Table, _super);
    function Table(param) {
        if (param === void 0) { param = {}; }
        var _this = _super.call(this) || this;
        _this.completeEvent = null;
        _this.hasPage = false;
        _this.pageSize = 20;
        _this.totalRow = 0;
        _this.currentPage = 1;
        _this.pageEvent = null;
        if (!param.hasOwnProperty("container")) {
            _super.prototype.log.call(_this, "参数：container 未设置");
            return _this;
        }
        _this.container = param["container"];
        if (!param.hasOwnProperty("layer")) {
            _super.prototype.log.call(_this, "参数：layer 未设置");
            return _this;
        }
        _this.layer = param["layer"];
        if (!param.hasOwnProperty("data") && !param.hasOwnProperty("ajax")) {
            _super.prototype.log.call(_this, "参数：data||ajax 未设置");
            return _this;
        }
        if (!param.hasOwnProperty("columns") || param.columns.length <= 0) {
            _super.prototype.log.call(_this, "参数：columns 未设置");
            return _this;
        }
        _this.columns = param["columns"];
        if (param.hasOwnProperty("completeEvent") && typeof (param.completeEvent) == "function") {
            _this.completeEvent = param.completeEvent;
        }
        if (param.hasOwnProperty("data") && param.data.length > 0) {
            _this.data = param["data"];
            _this.init();
        }
        else {
            var ajaxParam = param["ajax"];
            _this.container.addClass("loading");
            $.ajax({
                url: ajaxParam["url"],
                type: ajaxParam["type"],
                dataType: ajaxParam["dataType"],
                success: function (rs) {
                    if (rs.data.hasOwnProperty("list")) {
                        if (!rs.data.hasOwnProperty("total")) {
                            _super.prototype.log.call(_this, "参数：total 未设置");
                            return;
                        }
                        _this.totalRow = rs.data.total;
                        _this.data = rs.data.list;
                        _this.hasPage = true;
                        if (rs.data.hasOwnProperty("pageSize")) {
                            _this.pageSize = rs.data.pageSize;
                        }
                        if (rs.data.hasOwnProperty("currentPage")) {
                            _this.currentPage = rs.data.currentPage;
                        }
                        if (param.hasOwnProperty("pageEvent") && typeof (param.pageEvent) == "function") {
                            _this.pageEvent = param.pageEvent;
                        }
                    }
                    else {
                        _this.data = rs.data;
                    }
                    if (_this.data.length <= 0) {
                        _super.prototype.log.call(_this, "没有任何数据");
                        return;
                    }
                    _this.init();
                },
                error: function (request) {
                    if (Base.isEmpty(request.responseText)) {
                        _this.layer.msg(request.statusText, { icon: 2 });
                    }
                    else {
                        _this.layer.msg(request.responseJSON.msg, { icon: 2 });
                    }
                },
                complete: function (request) {
                    _this.container.removeClass("loading");
                }
            });
        }
        return _this;
    }
    Table.prototype.createTrHtml = function () {
        var html = "";
        for (var _i = 0, _a = this.data; _i < _a.length; _i++) {
            var item = _a[_i];
            html += "<tr>";
            for (var _b = 0, _c = this.columns; _b < _c.length; _b++) {
                var column = _c[_b];
                if (column.hasOwnProperty("event") && typeof (column.event) == "function") {
                    try {
                        if (Base.isEmpty(column.fields)) {
                            html += "<td>" + column.event(item) + "</td>";
                        }
                        else {
                            html += "<td>" + column.event(item[column.fields]) + "</td>";
                        }
                    }
                    catch (e) {
                        html += "<td></td>";
                    }
                }
                else if (!Base.isEmpty(column.fields)) {
                    html += "<td><label>" + item[column.fields] + "</label></td>";
                }
            }
            html += "</tr>";
        }
        return html;
    };
    Table.prototype.createPageHtml = function () {
        var totalPage = this.totalRow > this.pageSize ? parseInt(Math.ceil(this.totalRow / this.pageSize).toString()) : 1;
        if (this.currentPage > totalPage) {
            this.currentPage = totalPage;
        }
        var num = 7;
        var zj = parseInt((num / 2).toString());
        var start = 1;
        var end = totalPage;
        if (totalPage > num) {
            end = num;
            if (this.currentPage > zj + 1 && this.currentPage < totalPage - zj) {
                start = this.currentPage - zj;
                end = this.currentPage + zj;
            }
            else if (this.currentPage >= totalPage - zj && this.currentPage <= totalPage) {
                start = totalPage - (zj * 2);
                end = totalPage;
            }
        }
        var html = "<div id='page' class='ui small basic icon buttons'>";
        if (this.currentPage > 1) {
            html += "<button class=\"ui button\" onclick=\"return " + this.pageEvent + "(1);\"><i class='angle double left icon'></i>\u9996\u9875</button>";
            html += "<button class=\"ui button\" onclick=\"return " + this.pageEvent + "(" + this.currentPage + " - 1);\"><i class='angle left icon'></i>\u4E0A\u9875</button>";
        }
        else {
            html += "<button class='ui button' disabled><i class='angle double left icon'></i>首页</button>";
            html += "<button class='ui button' disabled><i class='angle left icon'></i>上页</button>";
        }
        for (var k = start; k <= end; k++) {
            if (this.currentPage == k) {
                html += "<button class='ui button' disabled>" + k + "</button>";
            }
            else {
                html += "<button class=\"ui button\" onclick=\"return " + this.pageEvent + "(" + k + ");\">" + k + "</button>";
            }
        }
        if (this.currentPage < totalPage) {
            html += "<button class=\"ui button\" onclick=\"return " + this.pageEvent + "(" + this.currentPage + " - 1);\"><i class='angle right icon'></i>\u4E0B\u9875</button>";
            html += "<button class=\"ui button\" onclick=\"return " + this.pageEvent + "(" + totalPage + ");\"><i class='angle double right icon'></i>\u5C3E\u9875</button>";
        }
        else {
            html += "<button class='ui button' disabled><i class='angle right icon'></i>下页</button>";
            html += "<button class='ui button' disabled><i class='angle double right icon'></i>尾页</button>";
        }
        html += "<button class='ui button' disabled>\u5171 " + this.pageEvent + " \u6761\u6570\u636E \uFF0C\u603B\u9875\u6570\uFF1A" + totalPage + "\uFF0C\u5F53\u524D\u7B2C " + this.currentPage + " \u9875</button>";
        html += "</div>";
        return html;
    };
    Table.prototype.init = function () {
        var trHtml = this.createTrHtml();
        $.each(this.container.find("tbody>tr"), function (idx, tr) {
            if ($(tr).attr("bind-attr") != "first") {
                $(tr).remove();
            }
        });
        this.container.find("tbody").append(trHtml);
        if (this.hasPage) {
            var pageObj = this.container.next("#page");
            if (pageObj.length > 0) {
                pageObj.remove();
            }
            var pageHtml = this.createPageHtml();
            this.container.after(pageHtml);
        }
        if (this.completeEvent != null) {
            this.completeEvent(this.container);
        }
    };
    return Table;
}(Base));
//# sourceMappingURL=tools.js.map