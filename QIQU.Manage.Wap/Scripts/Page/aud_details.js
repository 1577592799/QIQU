
var currId = 0;

$(function () {

    currId = $("#hidden_id").val();

    $("#span_remove").click(function () { DelEvent(); }); //删除
    $("#span_publish").click(function () { PublishEvent(); }); //发布

});

function DelEvent() {
    layer.open({
        content: '操作提示',
        btn: ['删除', '取消'],
        skin: 'footer',
        yes: function (index) {
            layer.open({ type: 2, content: '删除中...' });
            $.post("/auditing/delaud", { id: currId }, function (data) {
                if (data.state <= 0) {
                    layer.open({
                        content: data.error,
                        time: 1.5,
                        end: function () {
                            layer.closeAll();
                        }
                    });
                } else {
                    layer.open({
                        content: '删除成功',
                        time: 1.5,
                        end: function () {
                            layer.closeAll();
                            location.href = "/auditing/index";
                        }
                    });
                }
            });
        }
    });
}

function PublishEvent() {

    var cateHtml = "";
    $.post("/auditing/category", {}, function (data) {
        if (data != null) {

            for (var i = 0; i < data.length; i++) {
                cateHtml += '<div style="padding: 20px;" onclick="PublishAction(' + data[i].Id + ')">' + data[i].Name + '</div>';
            }

            layer.open({ content: cateHtml, skin: 'footer' });

        } else {
            return;
        }
    });
}

function PublishAction(cateid) {
    layer.open({ type: 2, content: '发布中...' });
    $.post("/auditing/publish", { id: currId, cateid: cateid }, function (data) {
        if (data.state <= 0) {
            layer.open({
                content: data.error,
                time: 1.5,
                end: function () {
                    layer.closeAll();
                }
            });
        } else {
            layer.open({
                content: '发布成功',
                time: 1.5,
                end: function () {
                    layer.closeAll();
                    location.href = "/auditing/index";
                }
            });
        }
    });
}