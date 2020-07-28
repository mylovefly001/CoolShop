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
        this._debug = false;
    }
    Base.prototype.debug = function (t) {
        if (t === void 0) { t = false; }
        this._debug = t;
    };
    /**
     * 校验字符串是否为空
     * @param str
     */
    Base.prototype.isEmpty = function (str) {
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
    Base.prototype.log = function (str) {
        if (str === void 0) { str = ""; }
        if (this.debug && !this.isEmpty(str)) {
            console.debug(str);
        }
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
var MenuAttr = /** @class */ (function () {
    function MenuAttr() {
    }
    return MenuAttr;
}());
var Tree = /** @class */ (function (_super) {
    __extends(Tree, _super);
    function Tree(param) {
        if (param === void 0) { param = {}; }
        var _this = _super.call(this) || this;
        /**
         * 是否显示子节点
         */
        _this.showChild = true;
        /**
         * 勾选类型，1=单选框，2=多选框
         */
        _this.type = 1;
        _this.event = null;
        if (param.hasOwnProperty("debug") && param["debug"] == true) {
            _super.prototype.debug.call(_this, param["debug"]);
        }
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
        if (param.hasOwnProperty("type") && param["type"] == 2) {
            _this.type = param["type"];
        }
        if (param.hasOwnProperty("click") && typeof (param['click']) == "function") {
            _this.event = param["click"];
        }
        if (param.hasOwnProperty("data")) {
            _this.data = param["data"];
            _this.init();
        }
        else {
            var ajaxParam = param["ajax"];
            $.ajax({
                url: ajaxParam["url"],
                type: ajaxParam["type"],
                dataType: ajaxParam["dataType"],
                success: function (rs) {
                    _this.data = rs["data"];
                    _this.init();
                },
                error: function (request) {
                    _this.layer.msg(request.responseJSON.msg, { icon: 2 });
                }
            });
        }
        return _this;
    }
    Tree.getClassName = function (idx, data) {
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
    Tree.prototype.getNodeHtml = function (data, hasChild, className) {
        if (hasChild === void 0) { hasChild = true; }
        if (className === void 0) { className = ""; }
        var html = "<div class=\"" + className + " node\" bind-id=\"" + data.id + "\">";
        html += "<span class=\"separate\"></span>";
        //如果是目录
        if (data.type == 1) {
            html += "<i style=\"line-height: 15px; cursor: pointer;\" class=\"icon folder " + ((this.showChild && hasChild) ? "open" : "") + " outline\" bind-tag=\"folder\"></i>";
        }
        else {
            html += "<i style=\"line-height: 15px;\" class=\"icon file alternate outline\"></i>";
        }
        //如果是单选框
        if (this.type == 1) {
            html += "<div class=\"ui radio checkbox\" style=\"width: 18px;\">\n                        <input type=\"radio\" id=\"checkbox_" + data.id + "\" name=\"treeCheckBox\">\n                    </div>";
        }
        else {
            html += "<div class=\"ui checkbox\" style=\"width: 20px;\">\n                        <input type=\"checkbox\" id=\"checkbox_" + data.id + "\" name=\"treeCheckBox\">\n                    </div>";
        }
        //如果是目录，并且有图标
        if (data.type == 1 && !_super.prototype.isEmpty.call(this, data.icon)) {
            html += "<i style=\"line-height: 15px;\" class=\"icon " + data.icon + "\"></i>";
        }
        if (data.status == 1) {
            html += "<label>" + data.text + "</label>";
        }
        else {
            html += "<label class=\"disabled\">" + data.text + "</label>";
        }
        html += "</div>";
        return html;
    };
    Tree.prototype.getNode = function (data) {
        if (data === void 0) { data = []; }
        var html = "";
        if (data.length <= 0) {
            return html;
        }
        for (var i = 0; i < data.length; i++) {
            var hasChild = data[i].hasOwnProperty("child") && data[i].child.length > 0;
            var className = Tree.getClassName(i, data);
            html += this.getNodeHtml(data[i], hasChild, className);
            if (hasChild) {
                html += "<div class=\"" + className + " child\" style=\"" + (this.showChild ? "" : "display:none;") + "\">" + this.getNode(data[i].child) + "</div>";
            }
        }
        return html;
    };
    Tree.prototype.init = function () {
        var that = this;
        var html = this.getNode(this.data);
        this.container.html(html);
        //当选择/取消选择勾选框时
        this.container.find(".ui.checkbox").checkbox({
            fireOnInit: true,
            onChange: function () {
                if (that.type == 2) {
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
                if (that.type == 2) {
                    var obj = $(this).parents(".node").next(".child").find(".ui.checkbox");
                    obj.checkbox("set checked");
                }
            },
            onUnchecked: function () {
                if (that.type == 2) {
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
        //如果有点击事件
        if (this.event != null && typeof (this.event) == "function") {
            this.container.find(".node>label").on("click", function () {
                that.event(this);
            });
        }
    };
    return Tree;
}(Base));
//# sourceMappingURL=tools.js.map