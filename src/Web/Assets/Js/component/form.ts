class Form extends Base {
    private static isAuto: any = null;

    /**
     * iframe重设大小
     * @param layer
     */
    private static iframeResize(layer) {
        let i = 0;
        this.isAuto = setInterval(() => {
            layer.iframeAuto(layer.getFrameIndex(window.name));
            i++;
            if (typeof (this.isAuto) === "number" && i == 10) {
                clearInterval(this.isAuto);
                this.isAuto = null;
            }
        }, 100);
    }

    public static Dialog(param: any = {}) {
        if (!param.hasOwnProperty("form")) {
            return false;
        }
        if (!param.hasOwnProperty("layer")) {
            return false;
        }
        if (!param.hasOwnProperty("ajaxUrl")) {
            return false;
        }
        let that = this;
        $(param["form"]).form({
            fields: param.hasOwnProperty("fields") ? param["fields"] : null,
            inline: param.hasOwnProperty("inline") ? param["inline"] : true,
            on: "change",
            onSuccess(event: JQuery.TriggeredEvent<HTMLElement>, fields: any): void {
                let loading = param["layer"].load(2);
                if (param.hasOwnProperty("before") && typeof (param["before"]) === "function") {
                    fields = param["before"](fields);
                }
                $.ajax({
                    url: param['ajaxUrl'],
                    type: "post",
                    data: fields,
                    dataType: "json",
                    success: (rs) => {
                        if (param.hasOwnProperty("success") && typeof (param["success"]) === "function") {
                            param["success"](rs["data"]);
                        }
                    },
                    error: (request) => {
                        if (param.hasOwnProperty("error") && typeof (param["error"]) === "function") {
                            param["error"](request.responseJSON.data);
                        } else {
                            param["layer"].msg(request.responseJSON.msg, {icon: 2});
                        }
                    },
                    complete: (request) => {
                        param["layer"].close(loading);
                        if (param.hasOwnProperty("complete") && typeof (param["complete"]) === "function") {
                            param["complete"](request.responseJSON.data);
                        }
                    }
                });
            },
            onValid(): void {
                if (param.hasOwnProperty("autoIframe") && param["autoIframe"] == true) {
                    that.iframeResize(param["layer"]);
                }
            },
            onInvalid(): void {
                if (param.hasOwnProperty("autoIframe") && param["autoIframe"] == true) {
                    that.iframeResize(param["layer"]);
                }
            }
        });
    }


    /**
     * sidebar里的表单提交
     * @param param
     * @constructor
     */
    public static Sidebar(param: any = {}) {
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
            onSuccess(event: JQuery.TriggeredEvent<HTMLElement>, fields: any): void {
                $(event.target).addClass("loading");
                if (param.hasOwnProperty("before") && typeof (param["before"]) === "function") {
                    fields = param["before"](fields);
                }
                $.ajax({
                    url: param['ajaxUrl'],
                    type: "post",
                    data: fields,
                    dataType: "json",
                    success: (rs) => {
                        if (param.hasOwnProperty("success") && typeof (param["success"]) === "function") {
                            param["success"](rs["data"]);
                        }
                    },
                    error: (request) => {
                        if (param.hasOwnProperty("error") && typeof (param["error"]) === "function") {
                            param["error"](request.responseJSON.data);
                        } else {
                            param["layer"].msg(request.responseJSON.msg, {icon: 2});
                        }
                    },
                    complete: (request) => {
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
    }
}