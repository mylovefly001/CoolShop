class Table extends Base {

    /**
     * 主容器名称
     */
    public container: JQuery;
    /**
     * 弹出窗口控件
     */
    public layer: any;

    public data: any;

    public columns: any;

    public completeEvent: Function = null;

    public hasPage: boolean = false;
    public pageSize: number = 20;
    public totalRow: number = 0;
    public currentPage: number = 1;
    public pageEvent: Function = null;

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
        if (!param.hasOwnProperty("data") && !param.hasOwnProperty("ajax")) {
            super.log("参数：data||ajax 未设置");
            return;
        }
        if (!param.hasOwnProperty("columns") || param.columns.length <= 0) {
            super.log("参数：columns 未设置");
            return;
        }
        this.columns = param["columns"];
        if (param.hasOwnProperty("completeEvent") && typeof (param.completeEvent) == "function") {
            this.completeEvent = param.completeEvent;
        }
        if (param.hasOwnProperty("data") && param.data.length > 0) {
            this.data = param["data"];
            this.init();
        } else {
            let ajaxParam = param["ajax"];
            this.container.addClass("loading");
            $.ajax({
                url: ajaxParam["url"],
                type: ajaxParam["type"],
                dataType: ajaxParam["dataType"],
                success: (rs) => {
                    if (rs.data.hasOwnProperty("list")) {
                        if (!rs.data.hasOwnProperty("total")) {
                            super.log("参数：total 未设置");
                            return;
                        }
                        this.totalRow = rs.data.total;
                        this.data = rs.data.list;
                        this.hasPage = true;
                        if (rs.data.hasOwnProperty("pageSize")) {
                            this.pageSize = rs.data.pageSize;
                        }
                        if (rs.data.hasOwnProperty("currentPage")) {
                            this.currentPage = rs.data.currentPage;
                        }
                        if (param.hasOwnProperty("pageEvent") && typeof (param.pageEvent) == "function") {
                            this.pageEvent = param.pageEvent;
                        }
                    } else {
                        this.data = rs.data;
                    }
                    if (this.data.length <= 0) {
                        super.log("没有任何数据");
                        return;
                    }
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

    private createTrHtml() {
        let html = "";
        for (let item of this.data) {
            html += `<tr>`;
            for (let column of this.columns) {
                if (column.hasOwnProperty("event") && typeof (column.event) == "function") {
                    try {
                        if (Base.isEmpty(column.fields)) {
                            html += `<td>${column.event(item)}</td>`;
                        } else {
                            html += `<td>${column.event(item[column.fields])}</td>`;
                        }
                    } catch (e) {
                        html += "<td></td>";
                    }
                } else if (!Base.isEmpty(column.fields)) {
                    html += `<td><label>${item[column.fields]}</label></td>`;
                }
            }
            html += `</tr>`;
        }
        return html;
    }

    private createPageHtml() {
        let totalPage = this.totalRow > this.pageSize ? parseInt(Math.ceil(this.totalRow / this.pageSize).toString()) : 1;
        if (this.currentPage > totalPage) {
            this.currentPage = totalPage;
        }
        let num = 7;
        let zj = parseInt((num / 2).toString());
        let start = 1;
        let end = totalPage;
        if (totalPage > num) {
            end = num;
            if (this.currentPage > zj + 1 && this.currentPage < totalPage - zj) {
                start = this.currentPage - zj;
                end = this.currentPage + zj;
            } else if (this.currentPage >= totalPage - zj && this.currentPage <= totalPage) {
                start = totalPage - (zj * 2);
                end = totalPage;
            }
        }
        let html = `<div id='page' class='ui small basic icon buttons'>`;
        if (this.currentPage > 1) {
            html += `<button class="ui button" onclick="return ${this.pageEvent}(1);"><i class='angle double left icon'></i>首页</button>`;
            html += `<button class="ui button" onclick="return ${this.pageEvent}(${this.currentPage} - 1);"><i class='angle left icon'></i>上页</button>`;
        } else {
            html += "<button class='ui button' disabled><i class='angle double left icon'></i>首页</button>";
            html += "<button class='ui button' disabled><i class='angle left icon'></i>上页</button>";
        }
        for (let k = start; k <= end; k++) {
            if (this.currentPage == k) {
                html += `<button class='ui button' disabled>${k}</button>`;
            } else {
                html += `<button class="ui button" onclick="return ${this.pageEvent}(${k});">${k}</button>`;
            }
        }
        if (this.currentPage < totalPage) {
            html += `<button class="ui button" onclick="return ${this.pageEvent}(${this.currentPage} - 1);"><i class='angle right icon'></i>下页</button>`;
            html += `<button class="ui button" onclick="return ${this.pageEvent}(${totalPage});"><i class='angle double right icon'></i>尾页</button>`;
        } else {
            html += "<button class='ui button' disabled><i class='angle right icon'></i>下页</button>";
            html += "<button class='ui button' disabled><i class='angle double right icon'></i>尾页</button>";
        }
        html += `<button class='ui button' disabled>共 ${this.pageEvent} 条数据 ，总页数：${totalPage}，当前第 ${this.currentPage} 页</button>`;
        html += "</div>";
        return html;
    }

    public init() {
        let trHtml = this.createTrHtml();
        $.each(this.container.find("tbody>tr"), (idx, tr) => {
            if ($(tr).attr("bind-attr") != "first") {
                $(tr).remove();
            }
        });
        this.container.find("tbody").append(trHtml);
        if (this.hasPage) {
            let pageObj = this.container.next("#page");
            if (pageObj.length > 0) {
                pageObj.remove();
            }
            let pageHtml = this.createPageHtml();
            this.container.after(pageHtml);
        }
        if (this.completeEvent != null) {
            this.completeEvent(this.container);
        }
    }
}