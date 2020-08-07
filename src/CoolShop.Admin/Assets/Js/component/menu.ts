class MenuTree {
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
     * 子节点
     */
    child: Array<MenuTree>;
}

class Menu extends Base {
    /**
     * 主容器名称
     */
    public container: JQuery;
    /**
     * 弹出窗口控件
     */
    public layer: any;

    /**
     * 数据源
     */
    public childData: Array<MenuTree>;

    /**
     * 是否显示子节点，true=显示，false=隐藏
     */
    public showChild: boolean = true;

    /**
     * 勾选类型，1=单选框，2=多选框
     */
    public checkType: number = 1;

    /**
     * 节点事件
     */
    public nodeEvent: any = null;

    constructor(param: any = {}) {
        super();
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
        if (!param.hasOwnProperty("childData") && !param.hasOwnProperty("ajax")) {
            super.log("参数：childData||ajax 未设置");
            return;
        }
        if (param.hasOwnProperty("checkType") && param["checkType"] == 2) {
            this.checkType = param["checkType"];
        }
        if (param.hasOwnProperty("nodeEvent") && typeof (param['nodeEvent']) == "function") {
            this.nodeEvent = param["nodeEvent"];
        }
        if (param.hasOwnProperty("childData")) {
            this.childData = param["childData"];
            this.init();
        } else {
            let ajaxParam = param["ajax"];
            this.container.addClass("loading");
            $.ajax({
                url: ajaxParam["url"],
                type: ajaxParam["type"],
                dataType: ajaxParam["dataType"],
                success: (rs) => {
                    this.childData = rs["data"];
                    this.init();
                },
                error: (request) => {
                    if (Base.isEmpty(request.responseText)) {
                        this.layer.msg(request.statusText, {icon: 2});
                    } else {
                        this.layer.msg(request.responseJSON.msg, {icon: 2});
                    }
                },
                complete: (request) => {
                    this.container.removeClass("loading");
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

    private getNodeHtml(dataInfo: MenuTree, hasChild: boolean = true, className: string = "") {
        let html = `<div class="${className} node" bind-id="${dataInfo.id}">`;
        html += `<span class="separate"></span>`;
        //如果是目录
        if (dataInfo.type == 1) {
            html += `<i style="line-height: 15px; cursor: pointer;" class="icon folder ${(this.showChild && hasChild) ? "open" : ""} outline" bind-tag="folder"></i>`;
        } else {
            html += `<i style="line-height: 15px;" class="icon file alternate outline"></i>`;
        }
        html += `<div class="ui ${this.checkType == 1 ? "radio" : ""} checkbox" style="width: 20px;">
                        <input type="${this.checkType == 1 ? "radio" : "checkbox"}" id="checkbox_${dataInfo.id}" name="treeCheckBox">
                    </div>`;
        //如果是目录，并且有图标
        if (dataInfo.type == 1 && !Base.isEmpty(dataInfo.icon)) {
            html += `<i style="line-height: 15px;" class="icon ${dataInfo.icon}"></i>`;
        }
        if (dataInfo.status == 1) {
            html += `<label>${dataInfo.text}</label>`;
        } else {
            html += `<label class="disabled">${dataInfo.text}</label>`;
        }
        html += `</div>`;
        return html;
    }

    private getNode(dataList: Array<MenuTree> = []) {
        let html = "";
        if (dataList.length <= 0) {
            return html;
        }
        for (let i = 0; i < dataList.length; i++) {
            let hasChild = dataList[i].hasOwnProperty("child") && dataList[i].child.length > 0;
            let className = Menu.getClassName(i, dataList);
            html += this.getNodeHtml(dataList[i], hasChild, className);
            if (hasChild) {
                html += `<div class="${className} child" style="${this.showChild ? "" : "display:none;"}">${this.getNode(dataList[i].child)}</div>`;
            }
        }
        return html;
    }

    public init() {
        let that = this;
        let html = this.getNode(this.childData);
        this.container.html(html);
        //当选择/取消选择勾选框时
        this.container.find(".ui.checkbox").checkbox({
            fireOnInit: true,
            onChange(): void {
                if (that.checkType == 2) {
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
                if (that.checkType == 2) {
                    let obj = $(this).parents(".node").next(".child").find(".ui.checkbox");
                    obj.checkbox("set checked");
                }
            },
            onUnchecked(): void {
                if (that.checkType == 2) {
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
        //移入/移出文字的时候颜色改变
        this.container.find(".node>label").mouseenter(function () {
            let color = $(this).css("color");
            $(this).css({"color": "#1678c2"}).attr("old-color", color);
        }).mouseleave(function () {
            let color = $(this).attr("old-color");
            $(this).css({"color": color});
        });
        //如果有点击事件
        if (this.nodeEvent != null && typeof (this.nodeEvent) == "function") {
            this.container.find(".node>label").on("click", function () {
                that.nodeEvent(this);
            });
        }
    }

    /**
     * 收缩动作
     * @param isExpand
     */
    public expand(isExpand: boolean = true) {
        //更改展开/关闭时的图标
        if (isExpand) {
            this.container.find(".child").show("fast", function () {
                $(this).prev(".node").children("i").addClass("open");
            });
        } else {
            this.container.find(".child").hide("fast", function () {
                $(this).prev(".node").children("i").removeClass("open");
            });
        }
    }

    /**
     * 全部选择或者全部取消
     * @param isSelect
     */
    public select(isSelect: boolean = true) {
        if (isSelect) {
            this.container.find(".ui.checkbox").checkbox("set checked");
        } else {
            this.container.find(".ui.checkbox").checkbox("set unchecked");
        }
    }

    /**
     * 获取选择的
     */
    public getSelectVal() {
        let ids = [];
        this.container.find(".ui.checkbox").each(function () {
            if ($(this).checkbox("is checked")) {
                let id = parseInt($(this).parent(".node").attr("bind-id"));
                ids.push(id);
            }
        });
        return ids;
    }
}