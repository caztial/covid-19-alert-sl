var AppConfig = {
    init(titleIn) {
        this.title = titleIn;
        this.setTitle();
        this.setActiveMenuItem();
    },
    setTitle() {
        $("title").text(this.title);
    },
    setActiveMenuItem() {
        $(".sidebar-wrapper a").each(function (i, v) {
            let elemRef = $(v);
            if (elemRef.hasClass("active")) {
                $(".sidebar-wrapper li, .sidebar-wrapper a").removeClass("active");
                elemRef.parent().addClass("active");
                return false;
            }
        });
    }
};