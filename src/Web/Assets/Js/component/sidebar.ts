class Sidebar extends Base {
    public static Init(param: any = {}) {
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
            let html = this.createHtml(param);
            param['container'].attr("bind-cmd", param['cmd']).html(html);
            if (param.hasOwnProperty("ajaxUrl")) {
                param['container'].find(".ui.form").addClass("loading");
                $.ajax({
                    url: param["ajaxUrl"],
                    type: "get",
                    dataType: "json",
                    success: (rs) => {
                        param['container'].find(".ui.form").removeClass("loading");
                        
                    },
                    error: (request) => {
                        param["layer"].msg(request.responseJSON.msg, {icon: 2});
                        return false;
                    }
                });
            }
            if (param.hasOwnProperty("event") && typeof (param["event"]) === "function") {
                param["event"](param['container']);
            }
            param['container'].find("i[bind-cmd='return']").on("click", () => {
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
    }

    private static createHtml(param: any = {}) {
        let html = `<div class="header"><i class="icon chevron left" bind-cmd="return" title="返回"></i>${param['title']}</div>
                    <div class="content">
                        <div class="ui form">`;
        $.each(param['data'], (key, item) => {
            if (!item.hasOwnProperty("des")) {
                item['des'] = item['label'];
            }
            switch (item['type']) {
                case 'input':
                    html += `<div class="field">
                                <label>${item['label']}：</label>
                                <input type="text" name="${item['name']}" placeholder="${item['des']}" autocomplete="off" value="${item['val']}" >
                            </div>`;
                    break;
                case 'icon':
                    html += `<div class="field">
                                <label>${item['label']}：</label>
                                <div class="ui right action left icon input">
                                    <i class="icon ${item['val']}"></i>
                                    <input type="text" name="${item['name']}" placeholder="${item['des']}" autocomplete="off" value="${item['val']}" >
                                   <button class="ui icon button">
                                      <i class="search icon"></i>
                                    </button>
                                </div>
                            </div>`;
                    break;
                case 'number':
                    html += `<div class="field">
                                <label>${item['label']}：</label>
                                <input type="text" name="${item['name']}" placeholder="${item['des']}" autocomplete="off" value="${item['val']}" style="width: 75px;" >
                            </div>`;
                    break;
                case 'password':
                    html += `<div class="field">
                                <label>${item['label']}：</label>
                                <input type="password" name="${item['name']}" placeholder="${item['des']}" autocomplete="off" value="${item['val']}" >
                            </div>`;
                    break;
                case 'toggle':
                    html += `<div class="field">
                                <div class="ui green toggle checkbox" bind-tag="${item['name']}" bind-val="${item['val']}">
                                    <input type="checkbox" name="${item['name']}" class="hidden">
                                    <label>${item['label']}</label>
                                </div>
                            </div>`;
                    break;
                case 'dropdown':
                    html += `<div class="field">
                                <label>${item['label']}：</label>
                                <div class="ui selection dropdown ${item['name']}">
                                    <input type="hidden" name="${item['name']}" value="${item['val']}">
                                    <i class="dropdown icon"></i>
                                    <div class="text">${item['des']}</div>
                                    <div class="menu">`;
                    $.each(item['list'], (k, v) => {
                        html += `<div class="item" data-value="${v['val']}">${v['text']}</div>`;
                    });
                    html += `</div> </div></div>`;
                    break;
            }
        });
        html += `<input type="hidden" name="id" value="${param['id']}">`;
        html += `<input type="hidden" name="cmd" value="${param['cmd']}">`;
        html += `<button class="ui fluid blue submit button">保存</button>`;
        html += `</div></div>`;
        return html;
    }
}