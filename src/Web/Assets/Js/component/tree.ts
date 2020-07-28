class MenuAttr {
    /**
     * ID
     */
    id: number;
    /**
     * 显示文字
     */
    text: string;
    /**
     * 显示图标
     */
    icon: string;
    /**
     * 状态
     */
    status: number;
    /**
     * 类型
     */
    type: number;
    /**
     * 其它属性
     */
    other: any;
    /**
     * 子节点
     */
    child: Array<MenuAttr>;
}

class Tree extends Base {
    /**
     * 主容器名称
     */
    public container: JQuery;
    /**
     * 弹出窗口控件
     */
    public layer: any;

    public data: Array<MenuAttr>;

    /**
     * 是否显示子节点
     */
    public showChild: boolean = true;

    /**
     * 勾选类型，1=单选框，2=多选框
     */
    public type: number = 1;

    public event: any = null;


    constructor(param: any = {}) {
        super();
        if (param.hasOwnProperty("debug") && param["debug"] == true) {
            super.debug(param["debug"]);
        }
        if (!param.hasOwnProperty("container")) {
            super.log("参数：container 未设置");
            return;
        }
        this.container = param["container"];
        if (!param.hasOwnProperty("layer")) {
            super.log("参数：layer 未设置");
            return;
        }
        this.layer = param["layer"];
        if (!param.hasOwnProperty("data") && !param.hasOwnProperty("ajax")) {
            super.log("参数：data||ajax 未设置");
            return;
        }
        if (param.hasOwnProperty("type") && param["type"] == 2) {
            this.type = param["type"];
        }
        if (param.hasOwnProperty("click") && typeof (param['click']) == "function") {
            this.event = param["click"];
        }
        if (param.hasOwnProperty("data")) {
            this.data = param["data"];
            this.init();
        } else {
            let ajaxParam = param["ajax"];
            $.ajax({
                url: ajaxParam["url"],
                type: ajaxParam["type"],
                dataType: ajaxParam["dataType"],
                success: (rs) => {
                    this.data = rs["data"];
                    this.init();
                },
                error: (request) => {
                    this.layer.msg(request.responseJSON.msg, {icon: 2});
                }
            });
        }
    }

    private static getClassName(idx: number = 0, data: any = {}) {
        let name = "";
        if (data.length == 1) {
            name += "last";
        } else {
            if (idx == 0) {
                name += "first";
            } else if (idx == data.length - 1) {
                name += "last";
            } else {
                name += "normal";
            }
        }
        return name;
    }

    private getNodeHtml(data: MenuAttr, hasChild: boolean = true, className: string = "") {
        let html = `<div class="${className} node" bind-id="${data.id}">`;
        html += `<span class="separate"></span>`;
        //如果是目录
        if (data.type == 1) {
            html += `<i style="line-height: 15px; cursor: pointer;" class="icon folder ${(this.showChild && hasChild) ? "open" : ""} outline" bind-tag="folder"></i>`;
        } else {
            html += `<i style="line-height: 15px;" class="icon file alternate outline"></i>`;
        }
        //如果是单选框
        if (this.type == 1) {
            html += `<div class="ui radio checkbox" style="width: 18px;">
                        <input type="radio" id="checkbox_${data.id}" name="treeCheckBox">
                    </div>`;
        } else {
            html += `<div class="ui checkbox" style="width: 20px;">
                        <input type="checkbox" id="checkbox_${data.id}" name="treeCheckBox">
                    </div>`;
        }
        //如果是目录，并且有图标
        if (data.type == 1 && !super.isEmpty(data.icon)) {
            html += `<i style="line-height: 15px;" class="icon ${data.icon}"></i>`;
        }
        if (data.status == 1) {
            html += `<label>${data.text}</label>`;
        } else {
            html += `<label class="disabled">${data.text}</label>`;
        }
        html += `</div>`;
        return html;
    }

    private getNode(data: Array<MenuAttr> = []) {
        let html = "";
        if (data.length <= 0) {
            return html;
        }
        for (let i = 0; i < data.length; i++) {
            let hasChild = data[i].hasOwnProperty("child") && data[i].child.length > 0;
            let className = Tree.getClassName(i, data);
            html += this.getNodeHtml(data[i], hasChild, className);
            if (hasChild) {
                html += `<div class="${className} child" style="${this.showChild ? "" : "display:none;"}">${this.getNode(data[i].child)}</div>`;
            }
        }
        return html;
    }

    private init() {
        let that = this;
        let html = this.getNode(this.data);
        this.container.html(html);
        //当选择/取消选择勾选框时
        this.container.find(".ui.checkbox").checkbox({
            fireOnInit: true,
            onChange(): void {
                if (that.type == 2) {
                    $.each($(this).parents(".child"), (idx: number, child) => {
                        let num = 0;
                        let childObj = $(child).find(".ui.checkbox");
                        $.each(childObj, (i, item) => {
                            if ($(item).checkbox("is checked")) {
                                num += 1;
                            }
                        });
                        let parentObj = $(child).prev(".node").find(".ui.checkbox");
                        //如果全部未选中，则是未选中状态
                        if (num == 0) {
                            parentObj.checkbox("set unchecked");
                        } else {
                            //如果部分选中，则是半掩状态
                            if (num < childObj.length) {
                                parentObj.checkbox("set indeterminate");
                            } else {
                                parentObj.checkbox("set checked");
                            }
                        }
                    });
                }
            },
            onChecked(): void {
                if (that.type == 2) {
                    let obj = $(this).parents(".node").next(".child").find(".ui.checkbox");
                    obj.checkbox("set checked");
                }
            },
            onUnchecked(): void {
                if (that.type == 2) {
                    let obj = $(this).parents(".node").next(".child").find(".ui.checkbox");
                    obj.checkbox("set unchecked");
                }
            }
        });
        //更改展开/关闭时的图标
        this.container.find("i[bind-tag='folder']").on("click", function () {
            let t = this;
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
    }
}