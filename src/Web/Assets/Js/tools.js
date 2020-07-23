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
                    type: "post",
                    data: fields,
                    dataType: "json",
                    success: function (rs) {
                        if (param.hasOwnProperty("success") && typeof (param["success"]) === "function") {
                            param["success"](rs["data"]);
                        }
                    },
                    error: function (request) {
                        if (param.hasOwnProperty("error") && typeof (param["error"]) === "function") {
                            param["error"](request.responseJSON.data);
                        }
                        else {
                            param["layer"].msg(request.responseJSON.msg, { icon: 2 });
                        }
                    },
                    complete: function (request) {
                        param["layer"].close(loading);
                        if (param.hasOwnProperty("complete") && typeof (param["complete"]) === "function") {
                            param["complete"](request.responseJSON.data);
                        }
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
    /**
     * sidebar里的表单提交
     * @param param
     * @constructor
     */
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
                    type: "post",
                    data: fields,
                    dataType: "json",
                    success: function (rs) {
                        if (param.hasOwnProperty("success") && typeof (param["success"]) === "function") {
                            param["success"](rs["data"]);
                        }
                    },
                    error: function (request) {
                        if (param.hasOwnProperty("error") && typeof (param["error"]) === "function") {
                            param["error"](request.responseJSON.data);
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
                        if (param.hasOwnProperty("complete") && typeof (param["complete"]) === "function") {
                            param["complete"](request.responseJSON.data);
                        }
                    }
                });
            }
        });
    };
    Form.isAuto = null;
    return Form;
}(Base));
var Sidebar = /** @class */ (function (_super) {
    __extends(Sidebar, _super);
    function Sidebar() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Sidebar.Init = function (param) {
        if (param === void 0) { param = {}; }
        if (!param.hasOwnProperty("container")) {
            param['container'] = $('.ui.right.sidebar');
        }
        if (!param.hasOwnProperty("cmd")) {
            param['cmd'] = "add";
        }
        if (param['container'].attr("bind-cmd") != param['cmd']) {
            if (!param.hasOwnProperty("id")) {
                param['id'] = 0;
            }
            if (!param.hasOwnProperty("title")) {
                param['title'] = "默认标题";
            }
            if (!param.hasOwnProperty("data")) {
                param['data'] = [];
            }
            if (!param.hasOwnProperty("closable")) {
                param['closable'] = false;
            }
            var html = this.createHtml(param);
            param['container'].attr("bind-cmd", param['cmd']).html(html);
            if (param.hasOwnProperty("ajaxUrl")) {
                param['container'].find(".ui.form").addClass("loading");
                $.ajax({
                    url: param["ajaxUrl"],
                    type: "get",
                    dataType: "json",
                    success: function (rs) {
                        param['container'].find(".ui.form").removeClass("loading");
                    },
                    error: function (request) {
                        param["layer"].msg(request.responseJSON.msg, { icon: 2 });
                        return false;
                    }
                });
            }
            if (param.hasOwnProperty("event") && typeof (param["event"]) === "function") {
                param["event"](param['container']);
            }
            param['container'].find("i[bind-cmd='return']").on("click", function () {
                param['container'].sidebar("hide");
            });
            param['container'].sidebar('setting', {
                closable: param['closable'],
                defaultTransition: {
                    computer: {
                        left: 'overlay',
                        right: 'overlay',
                        top: 'overlay',
                        bottom: 'overlay'
                    },
                    mobile: {
                        left: 'overlay',
                        right: 'overlay',
                        top: 'overlay',
                        bottom: 'overlay'
                    }
                }
            });
        }
        param['container'].sidebar('toggle');
    };
    Sidebar.createHtml = function (param) {
        if (param === void 0) { param = {}; }
        var html = "<div class=\"header\"><i class=\"icon chevron left\" bind-cmd=\"return\" title=\"\u8FD4\u56DE\"></i>" + param['title'] + "</div>\n                    <div class=\"content\">\n                        <div class=\"ui form\">";
        $.each(param['data'], function (key, item) {
            if (!item.hasOwnProperty("des")) {
                item['des'] = item['label'];
            }
            switch (item['type']) {
                case 'input':
                    html += "<div class=\"field\">\n                                <label>" + item['label'] + "\uFF1A</label>\n                                <input type=\"text\" name=\"" + item['name'] + "\" placeholder=\"" + item['des'] + "\" autocomplete=\"off\" value=\"" + item['val'] + "\" >\n                            </div>";
                    break;
                case 'icon':
                    html += "<div class=\"field\">\n                                <label>" + item['label'] + "\uFF1A</label>\n                                <div class=\"ui right action left icon input\">\n                                    <i class=\"icon " + item['val'] + "\"></i>\n                                    <input type=\"text\" name=\"" + item['name'] + "\" placeholder=\"" + item['des'] + "\" autocomplete=\"off\" value=\"" + item['val'] + "\" >\n                                   <button class=\"ui icon button\">\n                                      <i class=\"search icon\"></i>\n                                    </button>\n                                </div>\n                            </div>";
                    break;
                case 'number':
                    html += "<div class=\"field\">\n                                <label>" + item['label'] + "\uFF1A</label>\n                                <input type=\"text\" name=\"" + item['name'] + "\" placeholder=\"" + item['des'] + "\" autocomplete=\"off\" value=\"" + item['val'] + "\" style=\"width: 75px;\" >\n                            </div>";
                    break;
                case 'password':
                    html += "<div class=\"field\">\n                                <label>" + item['label'] + "\uFF1A</label>\n                                <input type=\"password\" name=\"" + item['name'] + "\" placeholder=\"" + item['des'] + "\" autocomplete=\"off\" value=\"" + item['val'] + "\" >\n                            </div>";
                    break;
                case 'toggle':
                    html += "<div class=\"field\">\n                                <div class=\"ui green toggle checkbox\" bind-tag=\"" + item['name'] + "\" bind-val=\"" + item['val'] + "\">\n                                    <input type=\"checkbox\" name=\"" + item['name'] + "\" class=\"hidden\">\n                                    <label>" + item['label'] + "</label>\n                                </div>\n                            </div>";
                    break;
                case 'dropdown':
                    html += "<div class=\"field\">\n                                <label>" + item['label'] + "\uFF1A</label>\n                                <div class=\"ui selection dropdown " + item['name'] + "\">\n                                    <input type=\"hidden\" name=\"" + item['name'] + "\" value=\"" + item['val'] + "\">\n                                    <i class=\"dropdown icon\"></i>\n                                    <div class=\"text\">" + item['des'] + "</div>\n                                    <div class=\"menu\">";
                    $.each(item['list'], function (k, v) {
                        html += "<div class=\"item\" data-value=\"" + v['val'] + "\">" + v['text'] + "</div>";
                    });
                    html += "</div> </div></div>";
                    break;
            }
        });
        html += "<input type=\"hidden\" name=\"id\" value=\"" + param['id'] + "\">";
        html += "<input type=\"hidden\" name=\"cmd\" value=\"" + param['cmd'] + "\">";
        html += "<button class=\"ui fluid blue submit button\">\u4FDD\u5B58</button>";
        html += "</div></div>";
        return html;
    };
    return Sidebar;
}(Base));
var Tree = /** @class */ (function (_super) {
    __extends(Tree, _super);
    function Tree() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Tree.Init = function (param) {
        if (param === void 0) { param = {}; }
        if (!param.hasOwnProperty("container")) {
            return false;
        }
        if (!param.hasOwnProperty("layer")) {
            return false;
        }
        var that = this;
        if (!param.hasOwnProperty("data")) {
            if (!param.hasOwnProperty("ajaxUrl")) {
                return false;
            }
            $.ajax({
                url: param['ajaxUrl'],
                type: "get",
                dataType: "json",
                success: function (rs) {
                    param['data'] = rs['data'];
                    that.createCheckBox(param);
                },
                error: function (request) {
                    if (param.hasOwnProperty("error") && typeof (param["error"]) === "function") {
                        param["error"](request.responseJSON.data);
                    }
                    else {
                        param["layer"].msg(request.responseJSON.msg, { icon: 2 });
                    }
                },
                complete: function (request) {
                    if (param.hasOwnProperty("complete") && typeof (param["complete"]) === "function") {
                        param["complete"](request.responseJSON.data);
                    }
                }
            });
        }
        else {
            that.createCheckBox(param);
        }
    };
    Tree.createCheckBox = function (param) {
        if (param === void 0) { param = {}; }
        if (param['data'].length <= 0) {
            return true;
        }
        var isShowChild = false;
        if (param.hasOwnProperty("showChild") && param['showChild'] == true) {
            isShowChild = true;
        }
        var html = this.getNodeHtml(param['data'], isShowChild);
        var container = param['container'];
        container.html(html);
        //当选择/取消选择勾选框时
        container.find(".ui.checkbox").checkbox({
            fireOnInit: true,
            onChange: function () {
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
                        parentObj.checkbox("set unchecked").find("input[type='checkbox']").attr({ "bind-select": 0 });
                    }
                    else {
                        //如果部分选中，则是半掩状态
                        if (num < childObj.length) {
                            parentObj.checkbox("set indeterminate").find("input[type='checkbox']").attr({ "bind-select": 0 });
                        }
                        else {
                            parentObj.checkbox("set checked").find("input[type='checkbox']").attr({ "bind-select": 1 });
                        }
                    }
                });
            },
            onChecked: function () {
                $(this).attr({ "bind-select": 1 });
                var obj = $(this).parents(".node").next(".child").find(".ui.checkbox");
                obj.checkbox("set checked").find("input[type='checkbox']").attr({ "bind-select": 1 });
            },
            onUnchecked: function () {
                $(this).attr({ "bind-select": 0 });
                var obj = $(this).parents(".node").next(".child").find(".ui.checkbox");
                obj.checkbox("set unchecked").find("input[type='checkbox']").attr({ "bind-select": 0 });
            }
        });
        //更改展开/关闭时的图标
        container.find(".folder").on("click", function () {
            var that = this;
            $(this).closest(".node").next(".child").toggle('fast', function () {
                $(this).css("display") == "block" ? $(that).addClass("open") : $(that).removeClass("open");
            });
        });
        //如果有点击事件
        if (param.hasOwnProperty("click") && typeof (param['click']) == "function") {
            container.find(".node>label").on("click", function () {
                param['click'](this);
            });
        }
    };
    /**
     * 获取节点的Class名称
     * @param idx
     * @param data
     */
    Tree.getNodeClassName = function (idx, data) {
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
    /**
     * 创建节点的HTML
     * @param hasChild
     * @param className
     * @param data
     */
    Tree.createNodeHtml = function (hasChild, className, data) {
        if (hasChild === void 0) { hasChild = false; }
        if (className === void 0) { className = ""; }
        if (data === void 0) { data = {}; }
        var html = "<div class=\"" + className + " node\">";
        html += "<span class=\"separate\"></span>";
        html += "<div class=\"ui checkbox\" style=\"vertical-align: middle; width: 20px;\">";
        html += "<input type=\"checkbox\" id=\"checkbox_" + data['id'] + "\" bind-id=\"" + data['id'] + "\" bind-select=\"0\">";
        html += "</div>";
        if (data.hasOwnProperty("type") && data['type'] == 1) {
            html += hasChild ? "<span class=\"folder\"></span>" : "<span class=\"empty folder\"></span>";
        }
        else {
            html += "<span class=\"file\"></span>";
        }
        if (data['status'] == 1) {
            html += "<label>" + data['name'] + "</label>";
        }
        else {
            html += "<label class=\"disabled\">" + data['name'] + "</label>";
        }
        html += "</div>";
        return html;
    };
    /**
     * 获取节点的HTML
     * @param data
     * @param isShow
     */
    Tree.getNodeHtml = function (data, isShow) {
        if (data === void 0) { data = {}; }
        if (isShow === void 0) { isShow = false; }
        var that = this;
        var html = "";
        $.each(data, function (idx, item) {
            var hasChild = item.hasOwnProperty("child") && item['child'].length > 0;
            var className = that.getNodeClassName(idx, data);
            html += that.createNodeHtml(hasChild, className, item);
            if (hasChild) {
                html += "<div class=\"" + className + " child\" style=\"" + (isShow ? "" : "display:none;") + "\">" + that.getNodeHtml(item['child']) + "</div>";
            }
        });
        return html;
    };
    return Tree;
}(Base));
//# sourceMappingURL=tools.js.map