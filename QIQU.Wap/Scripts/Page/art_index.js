
$(function () {

    $(window).scroll(function () {
        //$(window).scrollTop()这个方法是当前滚动条滚动的距离
        //$(window).height()获取当前窗体的高度
        //$(document).height()获取当前文档的高度
        var bot = 50; //bot是底部距离的高度
        if ((bot + $(window).scrollTop()) >= ($(document).height() - $(window).height())) {
            //当底部基本距离+滚动的高度〉=文档的高度-窗体的高度时；
            if (Index.isRuning == false) {
                Index.NextPage();
            }
        }
    });

    var Index = {
        //是否正在读取
        isRuning: false,
        currPage: 1,
        //初始页显示条数
        initCount: 20,
        //数据记录数
        totalRecord: $("#hidden_count").val(),

        // 条件初始化
        categoryWhere: $("#hidden_category").val(),
        //html内容容器
        htmlContainer: $(".recommend_moudule"),

        //获取列表
        Articles: function () {
            //判断是否存在运行
            if (this.isRuning) return;
            //判断是否还有没有下一页的数据
            if (!this.IsNext()) return;

            var currThis = this;
            currThis.Loading();
            currThis.isRuning = true;

            //每次加载只加载8条数据,在后台代码pageCount中设置的
            $.post("/articles", { page: this.currPage, cate: currThis.categoryWhere }, function (data) {
                currThis.BindHtml(data);
                currThis.Closing();
                currThis.isRuning = false;
            });
        },

        //jsonData格式为{list:...,count:0}
        BindHtml: function (jsonData) {
            if (jsonData == null || jsonData.list == null) return;

            var html = "";
            for (var i = 0; i < jsonData.list.length; i++) {
                //console.log(jsonData.list[i].ImgUrl);
                html += "<a href='/article/" + jsonData.list[i].Id + "'>";
                html += "    <dl class='clearfix'>";
                html += "            <dt class='j_art_lazy' style='background-image: url(\"" + jsonData.list[i].ImgUrl + "');\"></dt>";
                html += "        <dd>";
                html += "            <h3 class='title'>" + jsonData.list[i].Title + "</h3>";
                html += "            <div class='mark_count'><time>" + jsonData.list[i].CreateTime + "</time><span class='comment-count'>" + jsonData.list[i].ReadCount + " 阅读</span></div>";
                html += "        </dd>";
                html += "    </dl>";
                html += "</a>";
            }

            this.totalRecord = jsonData.count;
            if (this.currPage == 1)
                $(this.htmlContainer).html(html);
            else
                $(this.htmlContainer).append(html);
        },

        // 下一页是否有数据
        IsNext: function () {
            var pageNum = this.totalRecord % this.initCount > 0 ? Math.floor(this.totalRecord / this.initCount) + 1 : Math.floor(this.totalRecord / this.initCount);
            if (this.currPage >= pageNum) {
                return false;
            } else {
                return true;
            }
        },

        //下一页
        NextPage: function () {
            //判断是否存在运行
            if (this.isRuning) return;
            //判断是否还有没有下一页的数据
            if (!this.IsNext()) return;

            this.currPage = this.currPage + 1;
            this.Articles(this.categoryWhere);
        },

        //loading层
        Loading: function () {
            $("div.loading").css("display", "");
        },
        Closing: function () {
            $("div.loading").css("display", "none");
        }
    };
});