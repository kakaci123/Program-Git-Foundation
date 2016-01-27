(function (d) {
    var e, b, a;
    b = {
        BigImagePath: "Content/mediafiles/",
        BigImageBorderColour: "FFF"
    };
    function i(j) {
        var k = "";
        if (j != null) {
            if (j.lastIndexOf("/") == -1) {
                k = j
            } else {
                k = j.substring(j.lastIndexOf("/") + 1)
            }
        }
        return k
    }
    function g() {
        var j = {
            width: d(window).width(),
            height: d(window).height()
        };
        return j
    }
    function c(j, l) {
        var k = {
            AvailableWidth: 0,
            DrawingSide: "left"
        };
        if (j > (l - j)) {
            k.AvailableWidth = j;
            k.DrawingSide = "left"
        } else {
            k.AvailableWidth = (l - j);
            k.DrawingSide = "right"
        }
        k.AvailableWidth -= 30;
        return k
    }
    function f(k, o, l, p) {
        var j, n, m;
        n = k <= l.AvailableWidth;
        m = o <= p.height;
        j = {
            width: k,
            height: o
        };
        if (!n || !m) {
            if (!n && !m) {
                j.width = l.AvailableWidth;
                j.height = (j.width * o) / k;
                if (j.height > p.height) {
                    j.height = p.height;
                    j.width = (j.height * k) / o
                }
            } else {
                if (!n) {
                    j.width = l.AvailableWidth;
                    j.height = (j.width * o) / k
                } else {
                    if (!m) {
                        j.height = p.height;
                        j.width = (j.height * k) / o
                    }
                }
            }
        }
        j.width = j.width - 6;
        j.height = j.height - 6;
        return j
    }
    function h(n, m, l, k) {
        var j = n;
        if (j < 0) {
            j = 0
        }
        if (j < d(window).scrollTop()) {
            j = d(window).scrollTop()
        }
        if ((j + l.height + k) > m) {
            j = m - l.height - k
        }
        return j
    }
    a = {
        start: function () {
            d(e).mousemove(function (k) {
                var j, v, l, m, n, u, r, q, w, t, s, p, o;
                if (d("img#__RitrattKabbari__").length == 1) {
                    v = d("img#__RitrattKabbari__").css("width", "auto").css("height", "auto");
                    m = v.width();
                    n = v.height();
                    u = g();
                    r = (k.pageX - d(window).scrollLeft());
                    q = (k.pageY - d(window).scrollTop());
                    w = c(r, u.width);
                    p = f(m, n, w, u);
                    v.width(p.width);
                    v.height(p.height);
                    t = 0;
                    s = 0;
                    if (w.DrawingSide == "right") {
                        t = k.pageX + 30
                    } else {
                        t = k.pageX - 30 - p.width
                    }
                    s = k.pageY - (p.height / 2);
                    o = d(window).scrollTop() + u.height;
                    s = h(s, o, p, 6);
                    v.css("left", t);
                    v.css("top", s);
                    v.show()
                } else {
                    j = d("img#__Dalwaqt__");
                    if (j.length == 1) {
                        j.css("left", k.pageX).css("top", k.pageY + 20)
                    } else {
                        d("img#__Dalwaqt__").remove();
                        d("body").append(d('<img id="__Dalwaqt__" style="width:16px; height:16px" alt="Loading big image..." src="data:image/gif;base64,R0lGODlhEAALAPQAAP///wAAANra2tDQ0Orq6gYGBgAAAC4uLoKCgmBgYLq6uiIiIkpKSoqKimRkZL6+viYmJgQEBE5OTubm5tjY2PT09Dg4ONzc3PLy8ra2tqCgoMrKyu7u7gAAAAAAAAAAACH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCwAAACwAAAAAEAALAAAFLSAgjmRpnqSgCuLKAq5AEIM4zDVw03ve27ifDgfkEYe04kDIDC5zrtYKRa2WQgAh+QQJCwAAACwAAAAAEAALAAAFJGBhGAVgnqhpHIeRvsDawqns0qeN5+y967tYLyicBYE7EYkYAgAh+QQJCwAAACwAAAAAEAALAAAFNiAgjothLOOIJAkiGgxjpGKiKMkbz7SN6zIawJcDwIK9W/HISxGBzdHTuBNOmcJVCyoUlk7CEAAh+QQJCwAAACwAAAAAEAALAAAFNSAgjqQIRRFUAo3jNGIkSdHqPI8Tz3V55zuaDacDyIQ+YrBH+hWPzJFzOQQaeavWi7oqnVIhACH5BAkLAAAALAAAAAAQAAsAAAUyICCOZGme1rJY5kRRk7hI0mJSVUXJtF3iOl7tltsBZsNfUegjAY3I5sgFY55KqdX1GgIAIfkECQsAAAAsAAAAABAACwAABTcgII5kaZ4kcV2EqLJipmnZhWGXaOOitm2aXQ4g7P2Ct2ER4AMul00kj5g0Al8tADY2y6C+4FIIACH5BAkLAAAALAAAAAAQAAsAAAUvICCOZGme5ERRk6iy7qpyHCVStA3gNa/7txxwlwv2isSacYUc+l4tADQGQ1mvpBAAIfkECQsAAAAsAAAAABAACwAABS8gII5kaZ7kRFGTqLLuqnIcJVK0DeA1r/u3HHCXC/aKxJpxhRz6Xi0ANAZDWa+kEAA7AAAAAAAAAAAA" />').css("position", "absolute").css("z-index", "1000000").css("left", k.pageX).css("top", k.pageY + 20))
                    }
                    //l = b.BigImagePath + i(d(this).attr("src"));
                    l = d(this).attr("src");
                    v = d('<img id="__RitrattKabbari__" />');
                    v.load(function () {
                        var E, C, A, z, F, D, B, y, x;
                        E = d(this);
                        m = d(this).width();
                        n = d(this).height();
                        C = g();
                        A = (k.pageX - d(window).scrollLeft());
                        z = (k.pageY - d(window).scrollTop());
                        F = c(A, C.width);
                        y = f(m, n, F, C);
                        E.width(y.width);
                        E.height(y.height);
                        D = 0;
                        B = 0;
                        if (F.DrawingSide == "right") {
                            D = k.pageX + 30
                        } else {
                            D = k.pageX - 30 - y.width
                        }
                        B = k.pageY - (y.height / 2);
                        x = d(window).scrollTop() + C.height;
                        B = h(B, x, y, 6);
                        E.css("left", D);
                        E.css("top", B);
                        d("img#__Dalwaqt__").remove();
                        E.show()
                    });
                    v.error(function () {
                        if (d("img#__Dalwaqt__").length == 1) {
                            v.remove();
                            d("img#__Dalwaqt__").attr("alt", "An error occurred while loading the big image.").attr("src", "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAntJREFUeNqkk99v0lAUxw9t6SjIwhgsiBlGYCJotixhM5lZwl72svgyffWf4C/YX8CTfwCJyZ4MW+LD3syMv5bIi1kcwUHmNJmAgEKhLaUtnlMKgcU3b/Jtb3vP53vPvfdc22AwgP9pHD1e2GzA4ptkAwiidrG7gQpZcd9RH3CqHOpKxw/SM5ycu2b42C4I6fDycjS4tOQVZmcdlKHcbq9enZ9vXpyePlFlOYNxr6YyGMGCx7P3cGcnPuA4oadp0Gu1RmPOQDzu9IfD8/mjo73u8L9pwtAD0wqyDkd6dXs73pIkIZDNQqfTmZL5T1GEB6lUnJmZSRMzNsD17IYSiehvURTu5nLmlPcOD0EURVPUpxY7OAARTRYikag+3KfhEjSAR26/31tvNuFlNApPSyVQFAVC+/sgSRKU8JthGHidTILdbgff3JyXGESfM5bBosEwDg3X7Xa5IOvzQaFQgGq1ahrZ8JTeIMzTKfX7oEiSg5jJDEDDARXTlTALl2EAy7LmbDzPm7oxsdssxmpW38ygD/BDrNWUQb0Ogq5D7Ph4CuY4DlLFIjjpOAhQVYWYsQG6va9VKg03Bi5OwJ/X1uDTyoppQNool02Dtiw3iZk0yH1rNMq4U/Ll1pYJnyF8C8dIJ7GYme67SAQMw5ArvV6JmLGBgeXZ1fXMx3q94AWQv66vw238L1ii/gnCHoTznU5BMowMMcTaqFRplykoidV4h2HScZ6Pxnje62FZBwX90XWlqKrNgqqWLhDOYxVeUgESaxnQacyjfFhegQTA5gLAfbxcN61C+1kD+HIG8BanxS78QtWR1UYGjLXBzmv341+NLmIXJSFr/BVgAM8jHGX/y9T1AAAAAElFTkSuQmCC")
                        }
                    });
                    d("body").append(v);
                    v.attr("src", l).attr("alt", "").css("border", "solid 3px #" + b.BigImageBorderColour).css("position", "absolute").css("z-index", "1000000").css("display", "none")
                }
            });
            d(e).bind("mouseout mouseleave",
            function () {
                d("img#__RitrattKabbari__").remove();
                d("img#__Dalwaqt__").remove()
            });
            return d(e)
        }
    };
    d.fn.kabbar = function (j) {
        e = this;
        if (a[j]) {
            return a[j].apply(this, Array.prototype.slice.call(arguments, 1))
        } else {
            if (typeof j === "object") {
                d.extend(b, j);
                return a.start.apply(arguments)
            } else {
                if (!j) {
                    return a.start.apply(this, arguments)
                } else {
                    d.error("Method " + j + " does not exist on jQuery.kabbar")
                }
            }
        }
    }
})(jQuery);