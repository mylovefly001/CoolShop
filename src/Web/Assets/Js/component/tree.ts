class Tree extends Base {
    public static Init(param: any = {}) {
        if (!param.hasOwnProperty("container")) {
            return false;
        }
        if (!param.hasOwnProperty("layer")) {
            return false;
        }
        let that = this;
        if (!param.hasOwnProperty("data")) {
            if (!param.hasOwnProperty("ajaxUrl")) {
                return false;
            }
            $.ajax({
                url: param['ajaxUrl'],
                type: "get",
                dataType: "json",
                success: (rs) => {
                    param['data'] = rs['data'];
                    that.createCheckBox(param);
                },
                error: (request) => {
                    if (param.hasOwnProperty("error") && typeof (param["error"]) === "function") {
                        param["error"](request.responseJSON.data);
                    } else {
                        param["layer"].msg(request.responseJSON.msg, {icon: 2});
                    }
                },
                complete: (request) => {
                    if (param.hasOwnProperty("complete") && typeof (param["complete"]) === "function") {
                        param["complete"](request.responseJSON.data);
                    }
                }
            });
        } else {
            that.createCheckBox(param);
        }
    }

    private static createCheckBox(param: any = {}) {
        if (param['data'].length <= 0) {
            return true;
        }
        let isShowChild = false;
        if (param.hasOwnProperty("showChild") && param['showChild'] == true) {
            isShowChild = true;
        }
        let html = this.getNodeHtml(param['data'], isShowChild);
        let container = param['container'];
        container.html(html);
        //当选择/取消选择勾选框时
        container.find(".ui.checkbox").checkbox({
            fireOnInit: true,
            onChange(): void {
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
                        parentObj.checkbox("set unchecked").find("input[type='checkbox']").attr({"bind-select": 0});
                    } else {
                        //如果部分选中，则是半掩状态
                        if (num < childObj.length) {
                            parentObj.checkbox("set indeterminate").find("input[type='checkbox']").attr({"bind-select": 0});
                        } else {
                            parentObj.checkbox("set checked").find("input[type='checkbox']").attr({"bind-select": 1});
                        }
                    }
                });
            },
            onChecked(): void {
                $(this).attr({"bind-select": 1});
                let obj = $(this).parents(".node").next(".child").find(".ui.checkbox");
                obj.checkbox("set checked").find("input[type='checkbox']").attr({"bind-select": 1});
            },
            onUnchecked(): void {
                $(this).attr({"bind-select": 0});
                let obj = $(this).parents(".node").next(".child").find(".ui.checkbox");
                obj.checkbox("set unchecked").find("input[type='checkbox']").attr({"bind-select": 0});
            }
        });
        //更改展开/关闭时的图标
        container.find(".folder").on("click", function () {
            let that = this;
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
    }

    /**
     * 获取节点的Class名称
     * @param idx
     * @param data
     */
    private static getNodeClassName(idx: number = 0, data: any = {}) {
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

    /**
     * 创建节点的HTML
     * @param hasChild
     * @param className
     * @param data
     */
    private static createNodeHtml(hasChild: boolean = false, className: string = "", data: any = {}) {
        let html = `<div class="${className} node">`;
        html += `<span class="separate"></span>`;
        html += `<div class="ui checkbox" style="vertical-align: middle; width: 20px;">`;
        html += `<input type="checkbox" id="checkbox_${data['id']}" bind-id="${data['id']}" bind-select="0">`;
        html += `</div>`;
        if (data.hasOwnProperty("type") && data['type'] == 1) {
            html += hasChild ? `<span class="folder"></span>` : `<span class="empty folder"></span>`;
        } else {
            html += `<span class="file"></span>`;
        }
        if (data['status'] == 1) {
            html += `<label>${data['name']}</label>`;
        } else {
            html += `<label class="disabled">${data['name']}</label>`;
        }
        html += `</div>`;
        return html;
    }

    /**
     * 获取节点的HTML
     * @param data
     * @param isShow
     */
    private static getNodeHtml(data: any = {}, isShow: boolean = false) {
        let that = this;
        let html = "";
        $.each(data, (idx: number, item) => {
            let hasChild = item.hasOwnProperty("child") && item['child'].length > 0;
            let className = that.getNodeClassName(idx, data);
            html += that.createNodeHtml(hasChild, className, item);
            if (hasChild) {
                html += `<div class="${className} child" style="${isShow ? "" : "display:none;"}">${that.getNodeHtml(item['child'])}</div>`;
            }
        });
        return html;
    }
}