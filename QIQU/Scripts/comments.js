
$(function () {
    $(".com_btn").click(function () {
        if (Comment.isRuning == false) {
            Comment.Publishing(this);
        }
    });

    $(".cmt-btn2").click(function () {
        Comment.Show(this);
    });

    var Comment = {
        //是否正在读取
        isRuning: false,
        //当前信息id
        currentArticleId: $("#hidden_id").val(),
        //当前信息下的评论数量
        articleCommentCount: $("#hidden_commentcount").val(),
        //当前评论的回复数量
        commentChildCount: 0,

        //发布
        Publishing: function (clickObj) {
            //判断是否存在运行
            if (this.isRuning) return;

            var currThis = this;
            currThis.isRuning = true;

            var comid = $(clickObj).parents("li").attr("data-comment-id");
            if (comid == undefined) comid = 0;
            var $div_container = $(clickObj).parent().parent();
            var contents = $div_container.find("[name=txt_contents]").val();
            var $result = $div_container.find(".span_result");
            if (contents == undefined || contents.length <= 0 || contents.length > 200) {
                $result.html("<font color='red'>发言字数在200字以内</font>");
                currThis.isRuning = false;
                return;
            }

            $result.html("<font color='#93afca'>正在发布,请稍后...</font>");
            $.post("/home/publishcom", { artid: currThis.currentArticleId, comid: comid, contents: contents }, function (data) {
                if (data.state > 0) {
                    $result.html("<font color='#93afca'>发布成功</font>");
                    //绑定html
                    currThis.BindCommentHtml(comid, data.comment);
                } else {
                    $result.html("<font color='red'>" + data.error + "</font>");
                }

                Sleep(this, 1500);
                this.NextStep = function () {
                    $div_container.find("[name=txt_contents]").val("");
                    $result.html("");
                    if ($div_container.attr("class") == "info-reply-area") {
                        $div_container.css("display", "none");
                    }
                    currThis.isRuning = false;
                }
            });
        },

        //成功后...
        BindCommentHtml: function (comid, commentJson) {
            if (commentJson == null) return;

            var html = "";
            if (comid > 0) { //为回复
                if (this.commentChildCount <= 0) {
                    html += "<ul class='main-comment-list small'>";
                }
                html += "<li>";
                html += "    <span class='cmt-name-wrap'>";
                html += "        <a class='cmt-name'>" + commentJson.UserName + "</a>";
                html += "    </span>";
                html += "    <span class='cmt-title'>";
                html += "        <span class='location'>" + commentJson.FromArea + "</span>";
                html += "        <time class='time'>刚刚</time>";
                html += "    </span>";
                html += "    <div class='cmt-content'>" + commentJson.Contents + "</div>";
                html += "</li>";
                if (this.commentChildCount <= 0) {
                    html += "</ul>";
                }

                var $ul = $("div .main-comment ul li[data-comment-id=" + comid + "] .detail .cmt-content").next("ul");
                if ($ul.length > 0) {
                    $ul.append(html);
                } else {
                    $("div .main-comment ul li[data-comment-id=" + comid + "] .detail .cmt-content").after(html);
                }
                this.SetCommentCount(parseInt(this.commentChildCount)+1);
            } else { //为评论
                if (this.articleCommentCount <= 0) {
                    html += "<ul class='main-comment-list'>";
                }
                html += "<li data-comment-id='" + commentJson.CommentId + "'>";
                html += "    <span class='u-img'>";
                html += "        <img src='" + commentJson.UserImage + "' class='img'>";
                html += "    </span>";
                html += "    <div class='detail'>";
                html += "        <div>";
                html += "            <span class='cmt-name-wrap'>";
                html += "                <a class='cmt-name'>" + commentJson.UserName + "</a>";
                html += "            </span>";
                html += "            <span class='cmt-title'>";
                html += "                <span class='location'>" + commentJson.FromArea + "</span>";
                html += "                <time class='time'>刚刚</time>";
                html += "            </span>";
                html += "        </div>";
                html += "        <div class='cmt-content'>" + commentJson.Contents + "</div>";
                html += "    </div>";
                html += "</li>";
                if (this.articleCommentCount <= 0) {
                    html += "</ul>";
                }

                var $ul = $("div .main-comment").find("ul:first");
                if ($ul.length > 0) {
                    $ul.prepend(html);
                } else {
                    $("div .main-comment").html(html);
                }
            }
        },

        //点击显示的初始对象
        initClickObj: null,

        Show: function (clickObj) {
            this.initClickObj = clickObj;
            var $div = $(clickObj).parent().siblings(".info-reply-area");
            var childcount = $(clickObj).parent().find(".cmt-btn2:first").text();
            if (childcount == undefined) childcount = 0;
            if ($div.css("display") == "none") {
                $("div .info-reply-area").css("display", "none");
                $div.css("display", "");
                $div.find("[name=txt_contents]").focus();
                this.commentChildCount = childcount;
            } else {
                $div.css("display", "none");
                this.commentChildCount = 0;
            }
            //alert(this.commentChildCount);
        },

        SetCommentCount: function (cout) {
            if (this.initClickObj != null) {
                $(this.initClickObj).parent().find(".cmt-btn2:first").text(cout);
            }
        }

    };

});
