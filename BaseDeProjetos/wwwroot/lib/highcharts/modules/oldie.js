/*
 Highcharts JS v8.1.2 (2020-06-16)

 Old IE (v6, v7, v8) module for Highcharts v6+.

 (c) 2010-2019 Highsoft AS
 Author: Torstein Honsi

 License: www.highcharts.com/license
*/
(function (d) { "object" === typeof module && module.exports ? (d["default"] = d, module.exports = d) : "function" === typeof define && define.amd ? define("highcharts/modules/oldie", ["highcharts"], function (l) { d(l); d.Highcharts = l; return d }) : d("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (d) {
    function l(d, G, h, l) { d.hasOwnProperty(G) || (d[G] = l.apply(null, h)) } d = d ? d._modules : {}; l(d, "modules/oldie.src.js", [d["parts/Chart.js"], d["parts/Color.js"], d["parts/Globals.js"], d["parts/Pointer.js"], d["parts/SVGElement.js"],
    d["parts/SVGRenderer.js"], d["parts/Utilities.js"]], function (d, l, h, C, w, r, g) {
        var G = l.parse, u = h.deg2rad, f = h.doc, H = h.noop, D = h.svg, t = h.win, Q = g.addEvent, E = g.createElement, y = g.css, L = g.defined, M = g.discardElement, N = g.erase, v = g.extend, R = g.extendClass, O = g.getOptions, S = g.isArray, P = g.isNumber, F = g.isObject; l = g.merge; var T = g.offset, z = g.pick, q = g.pInt, U = g.uniqueKey; O().global.VMLRadialGradientURL = "http://code.highcharts.com/8.1.2/gfx/vml-radial-gradient.png"; f && !f.defaultView && (h.getStyle = g.getStyle = function (a, b) {
            var c =
                { width: "clientWidth", height: "clientHeight" }[b]; if (a.style[b]) return q(a.style[b]); "opacity" === b && (b = "filter"); if (c) return a.style.zoom = 1, Math.max(a[c] - 2 * g.getStyle(a, "padding"), 0); a = a.currentStyle[b.replace(/\-(\w)/g, function (a, b) { return b.toUpperCase() })]; "filter" === b && (a = a.replace(/alpha\(opacity=([0-9]+)\)/, function (a, b) { return b / 100 })); return "" === a ? 1 : q(a)
        }); D || (Q(w, "afterInit", function () { "text" === this.element.nodeName && this.css({ position: "absolute" }) }), C.prototype.normalize = function (a, b) {
            a = a ||
            t.event; a.target || (a.target = a.srcElement); b || (this.chartPosition = b = T(this.chart.container)); return v(a, { chartX: Math.round(Math.max(a.x, a.clientX - b.left)), chartY: Math.round(a.y) })
        }, d.prototype.ieSanitizeSVG = function (a) {
            return a = a.replace(/<IMG /g, "<image ").replace(/<(\/?)TITLE>/g, "<$1title>").replace(/height=([^" ]+)/g, 'height="$1"').replace(/width=([^" ]+)/g, 'width="$1"').replace(/hc-svg-href="([^"]+)">/g, 'xlink:href="$1"/>').replace(/ id=([^" >]+)/g, ' id="$1"').replace(/class=([^" >]+)/g, 'class="$1"').replace(/ transform /g,
                " ").replace(/:(path|rect)/g, "$1").replace(/style="([^"]+)"/g, function (a) { return a.toLowerCase() })
        }, d.prototype.isReadyToRender = function () { var a = this; return D || t != t.top || "complete" === f.readyState ? !0 : (f.attachEvent("onreadystatechange", function () { f.detachEvent("onreadystatechange", a.firstRender); "complete" === f.readyState && a.firstRender() }), !1) }, f.createElementNS || (f.createElementNS = function (a, b) { return f.createElement(b) }), h.addEventListenerPolyfill = function (a, b) {
            function c(a) {
                a.target = a.srcElement ||
                    t; b.call(e, a)
            } var e = this; e.attachEvent && (e.hcEventsIE || (e.hcEventsIE = {}), b.hcKey || (b.hcKey = U()), e.hcEventsIE[b.hcKey] = c, e.attachEvent("on" + a, c))
        }, h.removeEventListenerPolyfill = function (a, b) { this.detachEvent && (b = this.hcEventsIE[b.hcKey], this.detachEvent("on" + a, b)) }, d = {
            docMode8: f && 8 === f.documentMode, init: function (a, b) {
                var c = ["<", b, ' filled="f" stroked="f"'], e = ["position: ", "absolute", ";"], m = "div" === b; ("shape" === b || m) && e.push("left:0;top:0;width:1px;height:1px;"); e.push("visibility: ", m ? "hidden" : "visible");
                c.push(' style="', e.join(""), '"/>'); b && (c = m || "span" === b || "img" === b ? c.join("") : a.prepVML(c), this.element = E(c)); this.renderer = a
            }, add: function (a) { var b = this.renderer, c = this.element, e = b.box, m = a && a.inverted; e = a ? a.element || a : e; a && (this.parentGroup = a); m && b.invertChild(c, e); e.appendChild(c); this.added = !0; this.alignOnAdd && !this.deferUpdateTransform && this.updateTransform(); if (this.onAdd) this.onAdd(); this.className && this.attr("class", this.className); return this }, updateTransform: w.prototype.htmlUpdateTransform,
            setSpanRotation: function () { var a = this.rotation, b = Math.cos(a * u), c = Math.sin(a * u); y(this.element, { filter: a ? ["progid:DXImageTransform.Microsoft.Matrix(M11=", b, ", M12=", -c, ", M21=", c, ", M22=", b, ", sizingMethod='auto expand')"].join("") : "none" }) }, getSpanCorrection: function (a, b, c, e, m) {
                var d = e ? Math.cos(e * u) : 1, A = e ? Math.sin(e * u) : 0, I = z(this.elemHeight, this.element.offsetHeight); this.xCorr = 0 > d && -a; this.yCorr = 0 > A && -I; var k = 0 > d * A; this.xCorr += A * b * (k ? 1 - c : c); this.yCorr -= d * b * (e ? k ? c : 1 - c : 1); m && "left" !== m && (this.xCorr -=
                    a * c * (0 > d ? -1 : 1), e && (this.yCorr -= I * c * (0 > A ? -1 : 1)), y(this.element, { textAlign: m }))
            }, pathToVML: function (a) { for (var b = a.length, c = []; b--;)P(a[b]) ? c[b] = Math.round(10 * a[b]) - 5 : "Z" === a[b] ? c[b] = "x" : (c[b] = a[b], !a.isArc || "wa" !== a[b] && "at" !== a[b] || (c[b + 5] === c[b + 7] && (c[b + 7] += a[b + 7] > a[b + 5] ? 1 : -1), c[b + 6] === c[b + 8] && (c[b + 8] += a[b + 8] > a[b + 6] ? 1 : -1))); return c.join(" ") || "x" }, clip: function (a) {
                var b = this; if (a) { var c = a.members; N(c, b); c.push(b); b.destroyClip = function () { N(c, b) }; a = a.getCSS(b) } else b.destroyClip && b.destroyClip(),
                    a = { clip: b.docMode8 ? "inherit" : "rect(auto)" }; return b.css(a)
            }, css: w.prototype.htmlCss, safeRemoveChild: function (a) { a.parentNode && M(a) }, destroy: function () { this.destroyClip && this.destroyClip(); return w.prototype.destroy.apply(this) }, on: function (a, b) { this.element["on" + a] = function () { var a = t.event; a.target = a.srcElement; b(a) }; return this }, cutOffPath: function (a, b) { a = a.split(/[ ,]/); var c = a.length; if (9 === c || 11 === c) a[c - 4] = a[c - 2] = q(a[c - 2]) - 10 * b; return a.join(" ") }, shadow: function (a, b, c) {
                var e = [], d, n = this.element,
                A = this.renderer, I = n.style, k = n.path; k && "string" !== typeof k.value && (k = "x"); var g = k; if (a) {
                    var f = z(a.width, 3); var p = (a.opacity || .15) / f; for (d = 1; 3 >= d; d++) {
                        var h = 2 * f + 1 - 2 * d; c && (g = this.cutOffPath(k.value, h + .5)); var l = ['<shape isShadow="true" strokeweight="', h, '" filled="false" path="', g, '" coordsize="10 10" style="', n.style.cssText, '" />']; var x = E(A.prepVML(l), null, { left: q(I.left) + z(a.offsetX, 1), top: q(I.top) + z(a.offsetY, 1) }); c && (x.cutOff = h + 1); l = ['<stroke color="', a.color || "#000000", '" opacity="', p * d, '"/>'];
                        E(A.prepVML(l), null, null, x); b ? b.element.appendChild(x) : n.parentNode.insertBefore(x, n); e.push(x)
                    } this.shadows = e
                } return this
            }, updateShadows: H, setAttr: function (a, b) { this.docMode8 ? this.element[a] = b : this.element.setAttribute(a, b) }, getAttr: function (a) { return this.docMode8 ? this.element[a] : this.element.getAttribute(a) }, classSetter: function (a) { (this.added ? this.element : this).className = a }, dashstyleSetter: function (a, b, c) {
                (c.getElementsByTagName("stroke")[0] || E(this.renderer.prepVML(["<stroke/>"]), null, null, c))[b] =
                a || "solid"; this[b] = a
            }, dSetter: function (a, b, c) { var e = this.shadows; a = a || []; this.d = a.join && a.join(" "); c.path = a = this.pathToVML(a); if (e) for (c = e.length; c--;)e[c].path = e[c].cutOff ? this.cutOffPath(a, e[c].cutOff) : a; this.setAttr(b, a) }, fillSetter: function (a, b, c) { var e = c.nodeName; "SPAN" === e ? c.style.color = a : "IMG" !== e && (c.filled = "none" !== a, this.setAttr("fillcolor", this.renderer.color(a, c, b, this))) }, "fill-opacitySetter": function (a, b, c) {
                E(this.renderer.prepVML(["<", b.split("-")[0], ' opacity="', a, '"/>']), null, null,
                    c)
            }, opacitySetter: H, rotationSetter: function (a, b, c) { c = c.style; this[b] = c[b] = a; c.left = -Math.round(Math.sin(a * u) + 1) + "px"; c.top = Math.round(Math.cos(a * u)) + "px" }, strokeSetter: function (a, b, c) { this.setAttr("strokecolor", this.renderer.color(a, c, b, this)) }, "stroke-widthSetter": function (a, b, c) { c.stroked = !!a; this[b] = a; P(a) && (a += "px"); this.setAttr("strokeweight", a) }, titleSetter: function (a, b) { this.setAttr(b, a) }, visibilitySetter: function (a, b, c) {
                "inherit" === a && (a = "visible"); this.shadows && this.shadows.forEach(function (c) {
                    c.style[b] =
                    a
                }); "DIV" === c.nodeName && (a = "hidden" === a ? "-999em" : 0, this.docMode8 || (c.style[b] = a ? "visible" : "hidden"), b = "top"); c.style[b] = a
            }, xSetter: function (a, b, c) { this[b] = a; "x" === b ? b = "left" : "y" === b && (b = "top"); this.updateClipping ? (this[b] = a, this.updateClipping()) : c.style[b] = a }, zIndexSetter: function (a, b, c) { c.style[b] = a }, fillGetter: function () { return this.getAttr("fillcolor") || "" }, strokeGetter: function () { return this.getAttr("strokecolor") || "" }, classGetter: function () { return this.getAttr("className") || "" }
        }, d["stroke-opacitySetter"] =
            d["fill-opacitySetter"], h.VMLElement = d = R(w, d), d.prototype.ySetter = d.prototype.widthSetter = d.prototype.heightSetter = d.prototype.xSetter, C = {
                Element: d, isIE8: -1 < t.navigator.userAgent.indexOf("MSIE 8.0"), init: function (a, b, c) {
                    this.crispPolyLine = r.prototype.crispPolyLine; this.alignedObjects = []; var e = this.createElement("div").css({ position: "relative" }); var d = e.element; a.appendChild(e.element); this.isVML = !0; this.box = d; this.boxWrapper = e; this.gradients = {}; this.cache = {}; this.cacheKeys = []; this.imgCount = 0; this.setSize(b,
                        c, !1); if (!f.namespaces.hcv) { f.namespaces.add("hcv", "urn:schemas-microsoft-com:vml"); try { f.createStyleSheet().cssText = "hcv\\:fill, hcv\\:path, hcv\\:shape, hcv\\:stroke{ behavior:url(#default#VML); display: inline-block; } " } catch (n) { f.styleSheets[0].cssText += "hcv\\:fill, hcv\\:path, hcv\\:shape, hcv\\:stroke{ behavior:url(#default#VML); display: inline-block; } " } }
                }, isHidden: function () { return !this.box.offsetWidth }, clipRect: function (a, b, c, e) {
                    var d = this.createElement(), n = F(a); return v(d, {
                        members: [],
                        count: 0, left: (n ? a.x : a) + 1, top: (n ? a.y : b) + 1, width: (n ? a.width : c) - 1, height: (n ? a.height : e) - 1, getCSS: function (a) { var b = a.element, c = b.nodeName, e = a.inverted, d = this.top - ("shape" === c ? b.offsetTop : 0), m = this.left; b = m + this.width; var n = d + this.height; d = { clip: "rect(" + Math.round(e ? m : d) + "px," + Math.round(e ? n : b) + "px," + Math.round(e ? b : n) + "px," + Math.round(e ? d : m) + "px)" }; !e && a.docMode8 && "DIV" === c && v(d, { width: b + "px", height: n + "px" }); return d }, updateClipping: function () { d.members.forEach(function (a) { a.element && a.css(d.getCSS(a)) }) }
                    })
                },
                color: function (a, b, c, e) {
                    var d = this, n = /^rgba/, g, f, k = "none"; a && a.linearGradient ? f = "gradient" : a && a.radialGradient && (f = "pattern"); if (f) {
                        var h, l, p = a.linearGradient || a.radialGradient, q, r, x, u, t = ""; a = a.stops; var w = [], y = function () { g = ['<fill colors="' + w.join(",") + '" opacity="', r, '" o:opacity2="', q, '" type="', f, '" ', t, 'focus="100%" method="any" />']; E(d.prepVML(g), null, null, b) }; var v = a[0]; var z = a[a.length - 1]; 0 < v[0] && a.unshift([0, v[1]]); 1 > z[0] && a.push([1, z[1]]); a.forEach(function (a, b) {
                            n.test(a[1]) ? (J = G(a[1]),
                                h = J.get("rgb"), l = J.get("a")) : (h = a[1], l = 1); w.push(100 * a[0] + "% " + h); b ? (r = l, x = h) : (q = l, u = h)
                        }); if ("fill" === c) if ("gradient" === f) c = p.x1 || p[0] || 0, a = p.y1 || p[1] || 0, v = p.x2 || p[2] || 0, p = p.y2 || p[3] || 0, t = 'angle="' + (90 - 180 * Math.atan((p - a) / (v - c)) / Math.PI) + '"', y(); else {
                            k = p.r; var C = 2 * k, D = 2 * k, F = p.cx, H = p.cy, K = b.radialReference, B; k = function () {
                                K && (B = e.getBBox(), F += (K[0] - B.x) / B.width - .5, H += (K[1] - B.y) / B.height - .5, C *= K[2] / B.width, D *= K[2] / B.height); t = 'src="' + O().global.VMLRadialGradientURL + '" size="' + C + "," + D + '" origin="0.5,0.5" position="' +
                                    F + "," + H + '" color2="' + u + '" '; y()
                            }; e.added ? k() : e.onAdd = k; k = x
                        } else k = h
                    } else if (n.test(a) && "IMG" !== b.tagName) { var J = G(a); e[c + "-opacitySetter"](J.get("a"), c, b); k = J.get("rgb") } else k = b.getElementsByTagName(c), k.length && (k[0].opacity = 1, k[0].type = "solid"), k = a; return k
                }, prepVML: function (a) {
                    var b = this.isIE8; a = a.join(""); b ? (a = a.replace("/>", ' xmlns="urn:schemas-microsoft-com:vml" />'), a = -1 === a.indexOf('style="') ? a.replace("/>", ' style="display:inline-block;behavior:url(#default#VML);" />') : a.replace('style="',
                        'style="display:inline-block;behavior:url(#default#VML);')) : a = a.replace("<", "<hcv:"); return a
                }, text: r.prototype.html, path: function (a) { var b = { coordsize: "10 10" }; S(a) ? b.d = a : F(a) && v(b, a); return this.createElement("shape").attr(b) }, circle: function (a, b, c) { var e = this.symbol("circle"); F(a) && (c = a.r, b = a.y, a = a.x); e.isCircle = !0; e.r = c; return e.attr({ x: a, y: b }) }, g: function (a) { var b; a && (b = { className: "highcharts-" + a, "class": "highcharts-" + a }); return this.createElement("div").attr(b) }, image: function (a, b, c, e, d) {
                    var m =
                        this.createElement("img").attr({ src: a }); 1 < arguments.length && m.attr({ x: b, y: c, width: e, height: d }); return m
                }, createElement: function (a) { return "rect" === a ? this.symbol(a) : r.prototype.createElement.call(this, a) }, invertChild: function (a, b) { var c = this; b = b.style; var e = "IMG" === a.tagName && a.style; y(a, { flip: "x", left: q(b.width) - (e ? q(e.top) : 1), top: q(b.height) - (e ? q(e.left) : 1), rotation: -90 });[].forEach.call(a.childNodes, function (b) { c.invertChild(b, a) }) }, symbols: {
                    arc: function (a, b, c, e, d) {
                        var f = d.start, m = d.end, g = d.r || c ||
                            e; c = d.innerR; e = Math.cos(f); var k = Math.sin(f), h = Math.cos(m), l = Math.sin(m); if (0 === m - f) return ["x"]; f = ["wa", a - g, b - g, a + g, b + g, a + g * e, b + g * k, a + g * h, b + g * l]; d.open && !c && f.push("e", "M", a, b); f.push("at", a - c, b - c, a + c, b + c, a + c * h, b + c * l, a + c * e, b + c * k, "x", "e"); f.isArc = !0; return f
                    }, circle: function (a, b, c, e, d) { d && L(d.r) && (c = e = 2 * d.r); d && d.isCircle && (a -= c / 2, b -= e / 2); return ["wa", a, b, a + c, b + e, a + c, b + e / 2, a + c, b + e / 2, "e"] }, rect: function (a, b, c, d, f) { return r.prototype.symbols[L(f) && f.r ? "callout" : "square"].call(0, a, b, c, d, f) }
                }
            }, h.VMLRenderer =
            d = function () { this.init.apply(this, arguments) }, d.prototype = l(r.prototype, C), h.Renderer = d); r.prototype.getSpanWidth = function (a, b) { var c = a.getBBox(!0).width; !D && this.forExport && (c = this.measureSpanWidth(b.firstChild.data, a.styles)); return c }; r.prototype.measureSpanWidth = function (a, b) { var c = f.createElement("span"); a = f.createTextNode(a); c.appendChild(a); y(c, b); this.box.appendChild(c); b = c.offsetWidth; M(c); return b }
    }); l(d, "masters/modules/oldie.src.js", [], function () { })
});
//# sourceMappingURL=oldie.js.map