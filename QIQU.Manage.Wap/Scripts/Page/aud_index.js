
$(function () {
    var $ul = $("div#tab-switch-wrap ul");
    $ul.find("li[data-tab=mainNews]").click(function () { Index.SelectStatus(this); Index.Auditings(0); });   //最 新
    $ul.find("li[data-tab=recomNews]").click(function () { Index.SelectStatus(this); Index.Auditings(1); });//已发布
    $ul.find("li[data-tab=videoNews]").click(function () { Index.SelectStatus(this); Index.Auditings(-1); }); //回收站
    $("#a_next").click(function () { Index.NextPage(this); });

    var Index = {
        //初始页码
        currPage: 1,
        //初始页显示条数
        initCount: 8,
        //数据记录数
        totalRecord: $("#hidden_count").val(),
        //html内容容器
        ulContainer: $("div#div_audit_list ul"),
        //下一页对象
        nextObj: null,

        // 条件初始化
        statusWhere: 0,

        //获取列表
        Auditings: function (status) {
            var currThis = this;

            currThis.statusWhere = status;
            currThis.Loading();

            //每次加载只加载8条数据,在后台代码pageCount中设置的
            $.post("/auditing/list", { page: this.currPage, status: status }, function (data) {
                currThis.BindHtml(data);
                currThis.Closing();
            });
        },

        //改变选中状态
        SelectStatus: function (clickObj) {
            $(clickObj).siblings().removeClass("selected");
            $(clickObj).addClass("selected");

            this.currPage = 1; // 设置页码初始值为1
            this.NextObjAction();
        },

        // 下一页对象的操作（显示或者隐藏）
        NextObjAction: function () {
            if (this.nextObj != null) {
                var pageNum = this.totalRecord % this.initCount > 0 ? Math.floor(this.totalRecord / this.initCount) + 1 : Math.floor(this.totalRecord / this.initCount);
                if (this.currPage >= pageNum) {
                    $(this.nextObj).hide();
                } else {
                    $(this.nextObj).show();
                }
            }
        },

        //下一页
        NextPage: function (clickObj) {
            this.currPage = this.currPage + 1;
            this.nextObj = clickObj;
            this.NextObjAction();
            this.Auditings(this.statusWhere);
        },

        //jsonData格式为{list:...,count:0}
        BindHtml: function (jsonData) {
            if (jsonData == null || jsonData.list == null) return;

            var html = "";
            for (var i = 0; i < jsonData.list.length; i++) {
                html += "<li>";
                html += "   <a href='/Auditing/Details/" + jsonData.list[i].Id + "' class='a-lk'>";
                html += "       <div class='detail'>";
                html += "           <strong class='tit'>" + jsonData.list[i].Title + "</strong>";
                html += "           <div class='info'><span class='resource'>" + jsonData.list[i].Author + "</span><span class='resource'>" + jsonData.list[i].CreateTime + "</span></div>";
                html += "       </div>";
                html += "       <span class='u-img'><img src='" + jsonData.list[i].ImgUrl + "'></span>";
                html += "   </a>";
                html += "</li>";
            }

            this.totalRecord = jsonData.count;
            if (this.currPage == 1)
                $(this.ulContainer).html(html);
            else
                $(this.ulContainer).append(html);
        },

        //loading层
        Loading: function () {
            layer.open({ type: 2 });
        },
        //close层
        Closing: function () {
            layer.closeAll();
        }
    };
});
