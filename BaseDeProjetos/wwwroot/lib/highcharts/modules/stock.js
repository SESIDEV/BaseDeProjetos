/*
 Highstock JS v8.1.2 (2020-06-16)

 Highstock as a plugin for Highcharts

 (c) 2010-2019 Torstein Honsi

 License: www.highcharts.com/license
*/
(function (l) { "object" === typeof module && module.exports ? (l["default"] = l, module.exports = l) : "function" === typeof define && define.amd ? define("highcharts/modules/stock", ["highcharts"], function (K) { l(K); l.Highcharts = K; return l }) : l("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (l) {
    function K(l, u, B, t) { l.hasOwnProperty(u) || (l[u] = t.apply(null, B)) } l = l ? l._modules : {}; K(l, "parts/NavigatorAxis.js", [l["parts/Globals.js"], l["parts/Utilities.js"]], function (l, u) {
        var B = l.isTouchDevice, t = u.addEvent, E = u.correctFloat,
        e = u.defined, x = u.isNumber, q = u.pick, c = function () { function c(c) { this.axis = c } c.prototype.destroy = function () { this.axis = void 0 }; c.prototype.toFixedRange = function (c, v, C, z) { var A = this.axis, n = A.chart; n = n && n.fixedRange; var a = (A.pointRange || 0) / 2; c = q(C, A.translate(c, !0, !A.horiz)); v = q(z, A.translate(v, !0, !A.horiz)); A = n && (v - c) / n; e(C) || (c = E(c + a)); e(z) || (v = E(v - a)); .7 < A && 1.3 > A && (z ? c = v - n : v = c + n); x(c) && x(v) || (c = v = void 0); return { min: c, max: v } }; return c }(); return function () {
            function q() { } q.compose = function (q) {
                q.keepProps.push("navigatorAxis");
                t(q, "init", function () { this.navigatorAxis || (this.navigatorAxis = new c(this)) }); t(q, "zoom", function (c) { var q = this.chart.options, z = q.navigator, A = this.navigatorAxis, n = q.chart.pinchType, a = q.rangeSelector; q = q.chart.zoomType; this.isXAxis && (z && z.enabled || a && a.enabled) && ("y" === q ? c.zoomed = !1 : (!B && "xy" === q || B && "xy" === n) && this.options.range && (z = A.previousZoom, e(c.newMin) ? A.previousZoom = [this.min, this.max] : z && (c.newMin = z[0], c.newMax = z[1], A.previousZoom = void 0))); "undefined" !== typeof c.zoomed && c.preventDefault() })
            };
            q.AdditionsClass = c; return q
        }()
    }); K(l, "parts/ScrollbarAxis.js", [l["parts/Globals.js"], l["parts/Utilities.js"]], function (l, u) {
        var B = u.addEvent, t = u.defined, E = u.pick; return function () {
            function e() { } e.compose = function (e, q) {
                B(e, "afterInit", function () {
                    var c = this; c.options && c.options.scrollbar && c.options.scrollbar.enabled && (c.options.scrollbar.vertical = !c.horiz, c.options.startOnTick = c.options.endOnTick = !1, c.scrollbar = new q(c.chart.renderer, c.options.scrollbar, c.chart), B(c.scrollbar, "changed", function (q) {
                        var e =
                            E(c.options && c.options.min, c.min), v = E(c.options && c.options.max, c.max), C = t(c.dataMin) ? Math.min(e, c.min, c.dataMin) : e, z = (t(c.dataMax) ? Math.max(v, c.max, c.dataMax) : v) - C; t(e) && t(v) && (c.horiz && !c.reversed || !c.horiz && c.reversed ? (e = C + z * this.to, C += z * this.from) : (e = C + z * (1 - this.from), C += z * (1 - this.to)), E(this.options.liveRedraw, l.svg && !l.isTouchDevice && !this.chart.isBoosting) || "mouseup" === q.DOMType || !t(q.DOMType) ? c.setExtremes(C, e, !0, "mousemove" !== q.DOMType, q) : this.setRange(this.from, this.to))
                    }))
                }); B(e, "afterRender",
                    function () {
                        var c = Math.min(E(this.options.min, this.min), this.min, E(this.dataMin, this.min)), q = Math.max(E(this.options.max, this.max), this.max, E(this.dataMax, this.max)), e = this.scrollbar, l = this.axisTitleMargin + (this.titleOffset || 0), C = this.chart.scrollbarsOffsets, z = this.options.margin || 0; e && (this.horiz ? (this.opposite || (C[1] += l), e.position(this.left, this.top + this.height + 2 + C[1] - (this.opposite ? z : 0), this.width, this.height), this.opposite || (C[1] += z), l = 1) : (this.opposite && (C[0] += l), e.position(this.left + this.width +
                            2 + C[0] - (this.opposite ? 0 : z), this.top, this.width, this.height), this.opposite && (C[0] += z), l = 0), C[l] += e.size + e.options.margin, isNaN(c) || isNaN(q) || !t(this.min) || !t(this.max) || this.min === this.max ? e.setRange(0, 1) : (C = (this.min - c) / (q - c), c = (this.max - c) / (q - c), this.horiz && !this.reversed || !this.horiz && this.reversed ? e.setRange(C, c) : e.setRange(1 - c, 1 - C)))
                    }); B(e, "afterGetOffset", function () { var c = this.horiz ? 2 : 1, e = this.scrollbar; e && (this.chart.scrollbarsOffsets = [0, 0], this.chart.axisOffset[c] += e.size + e.options.margin) })
            };
            return e
        }()
    }); K(l, "parts/Scrollbar.js", [l["parts/Axis.js"], l["parts/Globals.js"], l["parts/ScrollbarAxis.js"], l["parts/Utilities.js"], l["parts/Options.js"]], function (l, u, B, t, E) {
        var e = t.addEvent, x = t.correctFloat, q = t.defined, c = t.destroyObjectProperties, v = t.fireEvent, J = t.merge, G = t.pick, C = t.removeEvent; t = E.defaultOptions; var z = u.hasTouch, A = u.isTouchDevice, n = u.swapXY = function (a, h) { h && a.forEach(function (g) { for (var h = g.length, a, m = 0; m < h; m += 2)a = g[m + 1], "number" === typeof a && (g[m + 1] = g[m + 2], g[m + 2] = a) }); return a };
        E = function () {
            function a(h, g, a) { this._events = []; this.from = this.chartY = this.chartX = 0; this.scrollbar = this.group = void 0; this.scrollbarButtons = []; this.scrollbarGroup = void 0; this.scrollbarLeft = 0; this.scrollbarRifles = void 0; this.scrollbarStrokeWidth = 1; this.to = this.size = this.scrollbarTop = 0; this.track = void 0; this.trackBorderWidth = 1; this.userOptions = {}; this.y = this.x = 0; this.chart = a; this.options = g; this.renderer = a.renderer; this.init(h, g, a) } a.prototype.addEvents = function () {
                var h = this.options.inverted ? [1, 0] : [0,
                    1], g = this.scrollbarButtons, a = this.scrollbarGroup.element, n = this.track.element, m = this.mouseDownHandler.bind(this), k = this.mouseMoveHandler.bind(this), d = this.mouseUpHandler.bind(this); h = [[g[h[0]].element, "click", this.buttonToMinClick.bind(this)], [g[h[1]].element, "click", this.buttonToMaxClick.bind(this)], [n, "click", this.trackClick.bind(this)], [a, "mousedown", m], [a.ownerDocument, "mousemove", k], [a.ownerDocument, "mouseup", d]]; z && h.push([a, "touchstart", m], [a.ownerDocument, "touchmove", k], [a.ownerDocument, "touchend",
                        d]); h.forEach(function (d) { e.apply(null, d) }); this._events = h
            }; a.prototype.buttonToMaxClick = function (h) { var g = (this.to - this.from) * G(this.options.step, .2); this.updatePosition(this.from + g, this.to + g); v(this, "changed", { from: this.from, to: this.to, trigger: "scrollbar", DOMEvent: h }) }; a.prototype.buttonToMinClick = function (h) { var g = x(this.to - this.from) * G(this.options.step, .2); this.updatePosition(x(this.from - g), x(this.to - g)); v(this, "changed", { from: this.from, to: this.to, trigger: "scrollbar", DOMEvent: h }) }; a.prototype.cursorToScrollbarPosition =
                function (h) { var g = this.options; g = g.minWidth > this.calculatedWidth ? g.minWidth : 0; return { chartX: (h.chartX - this.x - this.xOffset) / (this.barWidth - g), chartY: (h.chartY - this.y - this.yOffset) / (this.barWidth - g) } }; a.prototype.destroy = function () { var h = this.chart.scroller; this.removeEvents();["track", "scrollbarRifles", "scrollbar", "scrollbarGroup", "group"].forEach(function (g) { this[g] && this[g].destroy && (this[g] = this[g].destroy()) }, this); h && this === h.scrollbar && (h.scrollbar = null, c(h.scrollbarButtons)) }; a.prototype.drawScrollbarButton =
                    function (h) {
                        var g = this.renderer, a = this.scrollbarButtons, c = this.options, m = this.size; var k = g.g().add(this.group); a.push(k); k = g.rect().addClass("highcharts-scrollbar-button").add(k); this.chart.styledMode || k.attr({ stroke: c.buttonBorderColor, "stroke-width": c.buttonBorderWidth, fill: c.buttonBackgroundColor }); k.attr(k.crisp({ x: -.5, y: -.5, width: m + 1, height: m + 1, r: c.buttonBorderRadius }, k.strokeWidth())); k = g.path(n([["M", m / 2 + (h ? -1 : 1), m / 2 - 3], ["L", m / 2 + (h ? -1 : 1), m / 2 + 3], ["L", m / 2 + (h ? 2 : -2), m / 2]], c.vertical)).addClass("highcharts-scrollbar-arrow").add(a[h]);
                        this.chart.styledMode || k.attr({ fill: c.buttonArrowColor })
                    }; a.prototype.init = function (h, g, I) { this.scrollbarButtons = []; this.renderer = h; this.userOptions = g; this.options = J(a.defaultOptions, g); this.chart = I; this.size = G(this.options.size, this.options.height); g.enabled && (this.render(), this.addEvents()) }; a.prototype.mouseDownHandler = function (h) {
                        h = this.chart.pointer.normalize(h); h = this.cursorToScrollbarPosition(h); this.chartX = h.chartX; this.chartY = h.chartY; this.initPositions = [this.from, this.to]; this.grabbedCenter =
                            !0
                    }; a.prototype.mouseMoveHandler = function (h) { var g = this.chart.pointer.normalize(h), a = this.options.vertical ? "chartY" : "chartX", c = this.initPositions || []; !this.grabbedCenter || h.touches && 0 === h.touches[0][a] || (g = this.cursorToScrollbarPosition(g)[a], a = this[a], a = g - a, this.hasDragged = !0, this.updatePosition(c[0] + a, c[1] + a), this.hasDragged && v(this, "changed", { from: this.from, to: this.to, trigger: "scrollbar", DOMType: h.type, DOMEvent: h })) }; a.prototype.mouseUpHandler = function (a) {
                        this.hasDragged && v(this, "changed", {
                            from: this.from,
                            to: this.to, trigger: "scrollbar", DOMType: a.type, DOMEvent: a
                        }); this.grabbedCenter = this.hasDragged = this.chartX = this.chartY = null
                    }; a.prototype.position = function (a, g, c, n) {
                        var h = this.options.vertical, k = 0, d = this.rendered ? "animate" : "attr"; this.x = a; this.y = g + this.trackBorderWidth; this.width = c; this.xOffset = this.height = n; this.yOffset = k; h ? (this.width = this.yOffset = c = k = this.size, this.xOffset = g = 0, this.barWidth = n - 2 * c, this.x = a += this.options.margin) : (this.height = this.xOffset = n = g = this.size, this.barWidth = c - 2 * n, this.y += this.options.margin);
                        this.group[d]({ translateX: a, translateY: this.y }); this.track[d]({ width: c, height: n }); this.scrollbarButtons[1][d]({ translateX: h ? 0 : c - g, translateY: h ? n - k : 0 })
                    }; a.prototype.removeEvents = function () { this._events.forEach(function (a) { C.apply(null, a) }); this._events.length = 0 }; a.prototype.render = function () {
                        var a = this.renderer, g = this.options, c = this.size, e = this.chart.styledMode, m; this.group = m = a.g("scrollbar").attr({ zIndex: g.zIndex, translateY: -99999 }).add(); this.track = a.rect().addClass("highcharts-scrollbar-track").attr({
                            x: 0,
                            r: g.trackBorderRadius || 0, height: c, width: c
                        }).add(m); e || this.track.attr({ fill: g.trackBackgroundColor, stroke: g.trackBorderColor, "stroke-width": g.trackBorderWidth }); this.trackBorderWidth = this.track.strokeWidth(); this.track.attr({ y: -this.trackBorderWidth % 2 / 2 }); this.scrollbarGroup = a.g().add(m); this.scrollbar = a.rect().addClass("highcharts-scrollbar-thumb").attr({ height: c, width: c, r: g.barBorderRadius || 0 }).add(this.scrollbarGroup); this.scrollbarRifles = a.path(n([["M", -3, c / 4], ["L", -3, 2 * c / 3], ["M", 0, c / 4], ["L",
                            0, 2 * c / 3], ["M", 3, c / 4], ["L", 3, 2 * c / 3]], g.vertical)).addClass("highcharts-scrollbar-rifles").add(this.scrollbarGroup); e || (this.scrollbar.attr({ fill: g.barBackgroundColor, stroke: g.barBorderColor, "stroke-width": g.barBorderWidth }), this.scrollbarRifles.attr({ stroke: g.rifleColor, "stroke-width": 1 })); this.scrollbarStrokeWidth = this.scrollbar.strokeWidth(); this.scrollbarGroup.translate(-this.scrollbarStrokeWidth % 2 / 2, -this.scrollbarStrokeWidth % 2 / 2); this.drawScrollbarButton(0); this.drawScrollbarButton(1)
                    }; a.prototype.setRange =
                        function (a, g) {
                            var h = this.options, c = h.vertical, m = h.minWidth, k = this.barWidth, d, p = !this.rendered || this.hasDragged || this.chart.navigator && this.chart.navigator.hasDragged ? "attr" : "animate"; if (q(k)) {
                                a = Math.max(a, 0); var r = Math.ceil(k * a); this.calculatedWidth = d = x(k * Math.min(g, 1) - r); d < m && (r = (k - m + d) * a, d = m); m = Math.floor(r + this.xOffset + this.yOffset); k = d / 2 - .5; this.from = a; this.to = g; c ? (this.scrollbarGroup[p]({ translateY: m }), this.scrollbar[p]({ height: d }), this.scrollbarRifles[p]({ translateY: k }), this.scrollbarTop = m,
                                    this.scrollbarLeft = 0) : (this.scrollbarGroup[p]({ translateX: m }), this.scrollbar[p]({ width: d }), this.scrollbarRifles[p]({ translateX: k }), this.scrollbarLeft = m, this.scrollbarTop = 0); 12 >= d ? this.scrollbarRifles.hide() : this.scrollbarRifles.show(!0); !1 === h.showFull && (0 >= a && 1 <= g ? this.group.hide() : this.group.show()); this.rendered = !0
                            }
                        }; a.prototype.trackClick = function (a) {
                            var g = this.chart.pointer.normalize(a), h = this.to - this.from, c = this.y + this.scrollbarTop, m = this.x + this.scrollbarLeft; this.options.vertical && g.chartY >
                                c || !this.options.vertical && g.chartX > m ? this.updatePosition(this.from + h, this.to + h) : this.updatePosition(this.from - h, this.to - h); v(this, "changed", { from: this.from, to: this.to, trigger: "scrollbar", DOMEvent: a })
                        }; a.prototype.update = function (a) { this.destroy(); this.init(this.chart.renderer, J(!0, this.options, a), this.chart) }; a.prototype.updatePosition = function (a, g) { 1 < g && (a = x(1 - x(g - a)), g = 1); 0 > a && (g = x(g - a), a = 0); this.from = a; this.to = g }; a.defaultOptions = {
                            height: A ? 20 : 14, barBorderRadius: 0, buttonBorderRadius: 0, liveRedraw: void 0,
                            margin: 10, minWidth: 6, step: .2, zIndex: 3, barBackgroundColor: "#cccccc", barBorderWidth: 1, barBorderColor: "#cccccc", buttonArrowColor: "#333333", buttonBackgroundColor: "#e6e6e6", buttonBorderColor: "#cccccc", buttonBorderWidth: 1, rifleColor: "#333333", trackBackgroundColor: "#f2f2f2", trackBorderColor: "#f2f2f2", trackBorderWidth: 1
                        }; return a
        }(); u.Scrollbar || (t.scrollbar = J(!0, E.defaultOptions, t.scrollbar), u.Scrollbar = E, B.compose(l, E)); return u.Scrollbar
    }); K(l, "parts/Navigator.js", [l["parts/Axis.js"], l["parts/Chart.js"],
    l["parts/Color.js"], l["parts/Globals.js"], l["parts/NavigatorAxis.js"], l["parts/Options.js"], l["parts/Scrollbar.js"], l["parts/Utilities.js"]], function (l, u, B, t, E, e, x, q) {
        B = B.parse; var c = e.defaultOptions, v = q.addEvent, J = q.clamp, G = q.correctFloat, C = q.defined, z = q.destroyObjectProperties, A = q.erase, n = q.extend, a = q.find, h = q.isArray, g = q.isNumber, I = q.merge, D = q.pick, m = q.removeEvent, k = q.splat, d = t.hasTouch, p = t.isTouchDevice; e = t.Series; var r = function (b) {
            for (var f = [], y = 1; y < arguments.length; y++)f[y - 1] = arguments[y]; f =
                [].filter.call(f, g); if (f.length) return Math[b].apply(0, f)
        }; q = "undefined" === typeof t.seriesTypes.areaspline ? "line" : "areaspline"; n(c, {
            navigator: {
                height: 40, margin: 25, maskInside: !0, handles: { width: 7, height: 15, symbols: ["navigator-handle", "navigator-handle"], enabled: !0, lineWidth: 1, backgroundColor: "#f2f2f2", borderColor: "#999999" }, maskFill: B("#6685c2").setOpacity(.3).get(), outlineColor: "#cccccc", outlineWidth: 1, series: {
                    type: q, fillOpacity: .05, lineWidth: 1, compare: null, dataGrouping: {
                        approximation: "average", enabled: !0,
                        groupPixelWidth: 2, smoothed: !0, units: [["millisecond", [1, 2, 5, 10, 20, 25, 50, 100, 200, 500]], ["second", [1, 2, 5, 10, 15, 30]], ["minute", [1, 2, 5, 10, 15, 30]], ["hour", [1, 2, 3, 4, 6, 8, 12]], ["day", [1, 2, 3, 4]], ["week", [1, 2, 3]], ["month", [1, 3, 6]], ["year", null]]
                    }, dataLabels: { enabled: !1, zIndex: 2 }, id: "highcharts-navigator-series", className: "highcharts-navigator-series", lineColor: null, marker: { enabled: !1 }, threshold: null
                }, xAxis: {
                    overscroll: 0, className: "highcharts-navigator-xaxis", tickLength: 0, lineWidth: 0, gridLineColor: "#e6e6e6",
                    gridLineWidth: 1, tickPixelInterval: 200, labels: { align: "left", style: { color: "#999999" }, x: 3, y: -4 }, crosshair: !1
                }, yAxis: { className: "highcharts-navigator-yaxis", gridLineWidth: 0, startOnTick: !1, endOnTick: !1, minPadding: .1, maxPadding: .1, labels: { enabled: !1 }, crosshair: !1, title: { text: null }, tickLength: 0, tickWidth: 0 }
            }
        }); t.Renderer.prototype.symbols["navigator-handle"] = function (b, f, y, d, a) {
            b = (a && a.width || 0) / 2; f = Math.round(b / 3) + .5; a = a && a.height || 0; return [["M", -b - 1, .5], ["L", b, .5], ["L", b, a + .5], ["L", -b - 1, a + .5], ["L", -b -
                1, .5], ["M", -f, 4], ["L", -f, a - 3], ["M", f - 1, 4], ["L", f - 1, a - 3]]
        }; var w = function () {
            function b(f) { this.zoomedMin = this.zoomedMax = this.yAxis = this.xAxis = this.top = this.size = this.shades = this.rendered = this.range = this.outlineHeight = this.outline = this.opposite = this.navigatorSize = this.navigatorSeries = this.navigatorOptions = this.navigatorGroup = this.navigatorEnabled = this.left = this.height = this.handles = this.chart = this.baseSeries = void 0; this.init(f) } b.prototype.drawHandle = function (f, y, b, a) {
                var d = this.navigatorOptions.handles.height;
                this.handles[y][a](b ? { translateX: Math.round(this.left + this.height / 2), translateY: Math.round(this.top + parseInt(f, 10) + .5 - d) } : { translateX: Math.round(this.left + parseInt(f, 10)), translateY: Math.round(this.top + this.height / 2 - d / 2 - 1) })
            }; b.prototype.drawOutline = function (f, y, b, a) {
                var d = this.navigatorOptions.maskInside, g = this.outline.strokeWidth(), p = g / 2, H = g % 2 / 2; g = this.outlineHeight; var r = this.scrollbarHeight || 0, F = this.size, k = this.left - r, w = this.top; b ? (k -= p, b = w + y + H, y = w + f + H, H = [["M", k + g, w - r - H], ["L", k + g, b], ["L", k, b],
                ["L", k, y], ["L", k + g, y], ["L", k + g, w + F + r]], d && H.push(["M", k + g, b - p], ["L", k + g, y + p])) : (f += k + r - H, y += k + r - H, w += p, H = [["M", k, w], ["L", f, w], ["L", f, w + g], ["L", y, w + g], ["L", y, w], ["L", k + F + 2 * r, w]], d && H.push(["M", f - p, w], ["L", y + p, w])); this.outline[a]({ d: H })
            }; b.prototype.drawMasks = function (f, y, b, a) {
                var d = this.left, g = this.top, p = this.height; if (b) { var H = [d, d, d]; var r = [g, g + f, g + y]; var F = [p, p, p]; var k = [f, y - f, this.size - y] } else H = [d, d + f, d + y], r = [g, g, g], F = [f, y - f, this.size - y], k = [p, p, p]; this.shades.forEach(function (f, y) {
                    f[a]({
                        x: H[y],
                        y: r[y], width: F[y], height: k[y]
                    })
                })
            }; b.prototype.renderElements = function () {
                var f = this, y = f.navigatorOptions, b = y.maskInside, d = f.chart, a = d.renderer, g, p = { cursor: d.inverted ? "ns-resize" : "ew-resize" }; f.navigatorGroup = g = a.g("navigator").attr({ zIndex: 8, visibility: "hidden" }).add();[!b, b, !b].forEach(function (b, H) { f.shades[H] = a.rect().addClass("highcharts-navigator-mask" + (1 === H ? "-inside" : "-outside")).add(g); d.styledMode || f.shades[H].attr({ fill: b ? y.maskFill : "rgba(0,0,0,0)" }).css(1 === H && p) }); f.outline = a.path().addClass("highcharts-navigator-outline").add(g);
                d.styledMode || f.outline.attr({ "stroke-width": y.outlineWidth, stroke: y.outlineColor }); y.handles.enabled && [0, 1].forEach(function (b) { y.handles.inverted = d.inverted; f.handles[b] = a.symbol(y.handles.symbols[b], -y.handles.width / 2 - 1, 0, y.handles.width, y.handles.height, y.handles); f.handles[b].attr({ zIndex: 7 - b }).addClass("highcharts-navigator-handle highcharts-navigator-handle-" + ["left", "right"][b]).add(g); if (!d.styledMode) { var H = y.handles; f.handles[b].attr({ fill: H.backgroundColor, stroke: H.borderColor, "stroke-width": H.lineWidth }).css(p) } })
            };
            b.prototype.update = function (f) { (this.series || []).forEach(function (f) { f.baseSeries && delete f.baseSeries.navigatorSeries }); this.destroy(); I(!0, this.chart.options.navigator, this.options, f); this.init(this.chart) }; b.prototype.render = function (f, y, b, d) {
                var a = this.chart, p = this.scrollbarHeight, H, r = this.xAxis, k = r.pointRange || 0; var F = r.navigatorAxis.fake ? a.xAxis[0] : r; var w = this.navigatorEnabled, h, c = this.rendered; var m = a.inverted; var n = a.xAxis[0].minRange, e = a.xAxis[0].options.maxRange; if (!this.hasDragged || C(b)) {
                    f =
                    G(f - k / 2); y = G(y + k / 2); if (!g(f) || !g(y)) if (c) b = 0, d = D(r.width, F.width); else return; this.left = D(r.left, a.plotLeft + p + (m ? a.plotWidth : 0)); this.size = h = H = D(r.len, (m ? a.plotHeight : a.plotWidth) - 2 * p); a = m ? p : H + 2 * p; b = D(b, r.toPixels(f, !0)); d = D(d, r.toPixels(y, !0)); g(b) && Infinity !== Math.abs(b) || (b = 0, d = a); f = r.toValue(b, !0); y = r.toValue(d, !0); var z = Math.abs(G(y - f)); z < n ? this.grabbedLeft ? b = r.toPixels(y - n - k, !0) : this.grabbedRight && (d = r.toPixels(f + n + k, !0)) : C(e) && G(z - k) > e && (this.grabbedLeft ? b = r.toPixels(y - e - k, !0) : this.grabbedRight &&
                        (d = r.toPixels(f + e + k, !0))); this.zoomedMax = J(Math.max(b, d), 0, h); this.zoomedMin = J(this.fixedWidth ? this.zoomedMax - this.fixedWidth : Math.min(b, d), 0, h); this.range = this.zoomedMax - this.zoomedMin; h = Math.round(this.zoomedMax); b = Math.round(this.zoomedMin); w && (this.navigatorGroup.attr({ visibility: "visible" }), c = c && !this.hasDragged ? "animate" : "attr", this.drawMasks(b, h, m, c), this.drawOutline(b, h, m, c), this.navigatorOptions.handles.enabled && (this.drawHandle(b, 0, m, c), this.drawHandle(h, 1, m, c))); this.scrollbar && (m ? (m = this.top -
                            p, F = this.left - p + (w || !F.opposite ? 0 : (F.titleOffset || 0) + F.axisTitleMargin), p = H + 2 * p) : (m = this.top + (w ? this.height : -p), F = this.left - p), this.scrollbar.position(F, m, a, p), this.scrollbar.setRange(this.zoomedMin / (H || 1), this.zoomedMax / (H || 1))); this.rendered = !0
                }
            }; b.prototype.addMouseEvents = function () {
                var f = this, b = f.chart, a = b.container, p = [], g, r; f.mouseMoveHandler = g = function (b) { f.onMouseMove(b) }; f.mouseUpHandler = r = function (b) { f.onMouseUp(b) }; p = f.getPartsEvents("mousedown"); p.push(v(b.renderTo, "mousemove", g), v(a.ownerDocument,
                    "mouseup", r)); d && (p.push(v(b.renderTo, "touchmove", g), v(a.ownerDocument, "touchend", r)), p.concat(f.getPartsEvents("touchstart"))); f.eventsToUnbind = p; f.series && f.series[0] && p.push(v(f.series[0].xAxis, "foundExtremes", function () { b.navigator.modifyNavigatorAxisExtremes() }))
            }; b.prototype.getPartsEvents = function (f) { var b = this, a = [];["shades", "handles"].forEach(function (y) { b[y].forEach(function (d, p) { a.push(v(d.element, f, function (f) { b[y + "Mousedown"](f, p) })) }) }); return a }; b.prototype.shadesMousedown = function (f,
                b) {
                    f = this.chart.pointer.normalize(f); var y = this.chart, a = this.xAxis, d = this.zoomedMin, p = this.left, g = this.size, r = this.range, k = f.chartX; y.inverted && (k = f.chartY, p = this.top); if (1 === b) this.grabbedCenter = k, this.fixedWidth = r, this.dragOffset = k - d; else {
                        f = k - p - r / 2; if (0 === b) f = Math.max(0, f); else if (2 === b && f + r >= g) if (f = g - r, this.reversedExtremes) { f -= r; var w = this.getUnionExtremes().dataMin } else var h = this.getUnionExtremes().dataMax; f !== d && (this.fixedWidth = r, b = a.navigatorAxis.toFixedRange(f, f + r, w, h), C(b.min) && y.xAxis[0].setExtremes(Math.min(b.min,
                            b.max), Math.max(b.min, b.max), !0, null, { trigger: "navigator" }))
                    }
            }; b.prototype.handlesMousedown = function (f, b) { this.chart.pointer.normalize(f); f = this.chart; var y = f.xAxis[0], a = this.reversedExtremes; 0 === b ? (this.grabbedLeft = !0, this.otherHandlePos = this.zoomedMax, this.fixedExtreme = a ? y.min : y.max) : (this.grabbedRight = !0, this.otherHandlePos = this.zoomedMin, this.fixedExtreme = a ? y.max : y.min); f.fixedRange = null }; b.prototype.onMouseMove = function (f) {
                var b = this, a = b.chart, d = b.left, g = b.navigatorSize, r = b.range, k = b.dragOffset,
                w = a.inverted; f.touches && 0 === f.touches[0].pageX || (f = a.pointer.normalize(f), a = f.chartX, w && (d = b.top, a = f.chartY), b.grabbedLeft ? (b.hasDragged = !0, b.render(0, 0, a - d, b.otherHandlePos)) : b.grabbedRight ? (b.hasDragged = !0, b.render(0, 0, b.otherHandlePos, a - d)) : b.grabbedCenter && (b.hasDragged = !0, a < k ? a = k : a > g + k - r && (a = g + k - r), b.render(0, 0, a - k, a - k + r)), b.hasDragged && b.scrollbar && D(b.scrollbar.options.liveRedraw, t.svg && !p && !this.chart.isBoosting) && (f.DOMType = f.type, setTimeout(function () { b.onMouseUp(f) }, 0)))
            }; b.prototype.onMouseUp =
                function (f) {
                    var b = this.chart, a = this.xAxis, d = this.scrollbar, p = f.DOMEvent || f, g = b.inverted, r = this.rendered && !this.hasDragged ? "animate" : "attr", k = Math.round(this.zoomedMax), w = Math.round(this.zoomedMin); if (this.hasDragged && (!d || !d.hasDragged) || "scrollbar" === f.trigger) {
                        d = this.getUnionExtremes(); if (this.zoomedMin === this.otherHandlePos) var h = this.fixedExtreme; else if (this.zoomedMax === this.otherHandlePos) var c = this.fixedExtreme; this.zoomedMax === this.size && (c = this.reversedExtremes ? d.dataMin : d.dataMax); 0 === this.zoomedMin &&
                            (h = this.reversedExtremes ? d.dataMax : d.dataMin); a = a.navigatorAxis.toFixedRange(this.zoomedMin, this.zoomedMax, h, c); C(a.min) && b.xAxis[0].setExtremes(Math.min(a.min, a.max), Math.max(a.min, a.max), !0, this.hasDragged ? !1 : null, { trigger: "navigator", triggerOp: "navigator-drag", DOMEvent: p })
                    } "mousemove" !== f.DOMType && "touchmove" !== f.DOMType && (this.grabbedLeft = this.grabbedRight = this.grabbedCenter = this.fixedWidth = this.fixedExtreme = this.otherHandlePos = this.hasDragged = this.dragOffset = null); this.navigatorEnabled && (this.shades &&
                        this.drawMasks(w, k, g, r), this.outline && this.drawOutline(w, k, g, r), this.navigatorOptions.handles.enabled && Object.keys(this.handles).length === this.handles.length && (this.drawHandle(w, 0, g, r), this.drawHandle(k, 1, g, r)))
                }; b.prototype.removeEvents = function () { this.eventsToUnbind && (this.eventsToUnbind.forEach(function (f) { f() }), this.eventsToUnbind = void 0); this.removeBaseSeriesEvents() }; b.prototype.removeBaseSeriesEvents = function () {
                    var f = this.baseSeries || []; this.navigatorEnabled && f[0] && (!1 !== this.navigatorOptions.adaptToUpdatedData &&
                        f.forEach(function (f) { m(f, "updatedData", this.updatedDataHandler) }, this), f[0].xAxis && m(f[0].xAxis, "foundExtremes", this.modifyBaseAxisExtremes))
                }; b.prototype.init = function (f) {
                    var b = f.options, a = b.navigator, d = a.enabled, p = b.scrollbar, g = p.enabled; b = d ? a.height : 0; var k = g ? p.height : 0; this.handles = []; this.shades = []; this.chart = f; this.setBaseSeries(); this.height = b; this.scrollbarHeight = k; this.scrollbarEnabled = g; this.navigatorEnabled = d; this.navigatorOptions = a; this.scrollbarOptions = p; this.outlineHeight = b + k; this.opposite =
                        D(a.opposite, !(d || !f.inverted)); var w = this; d = w.baseSeries; p = f.xAxis.length; g = f.yAxis.length; var h = d && d[0] && d[0].xAxis || f.xAxis[0] || { options: {} }; f.isDirtyBox = !0; w.navigatorEnabled ? (w.xAxis = new l(f, I({ breaks: h.options.breaks, ordinal: h.options.ordinal }, a.xAxis, { id: "navigator-x-axis", yAxis: "navigator-y-axis", isX: !0, type: "datetime", index: p, isInternal: !0, offset: 0, keepOrdinalPadding: !0, startOnTick: !1, endOnTick: !1, minPadding: 0, maxPadding: 0, zoomEnabled: !1 }, f.inverted ? { offsets: [k, 0, -k, 0], width: b } : {
                            offsets: [0,
                                -k, 0, k], height: b
                        })), w.yAxis = new l(f, I(a.yAxis, { id: "navigator-y-axis", alignTicks: !1, offset: 0, index: g, isInternal: !0, zoomEnabled: !1 }, f.inverted ? { width: b } : { height: b })), d || a.series.data ? w.updateNavigatorSeries(!1) : 0 === f.series.length && (w.unbindRedraw = v(f, "beforeRedraw", function () { 0 < f.series.length && !w.series && (w.setBaseSeries(), w.unbindRedraw()) })), w.reversedExtremes = f.inverted && !w.xAxis.reversed || !f.inverted && w.xAxis.reversed, w.renderElements(), w.addMouseEvents()) : (w.xAxis = {
                            chart: f, navigatorAxis: { fake: !0 },
                            translate: function (b, a) { var y = f.xAxis[0], d = y.getExtremes(), p = y.len - 2 * k, g = r("min", y.options.min, d.dataMin); y = r("max", y.options.max, d.dataMax) - g; return a ? b * y / p + g : p * (b - g) / y }, toPixels: function (f) { return this.translate(f) }, toValue: function (f) { return this.translate(f, !0) }
                        }, w.xAxis.navigatorAxis.axis = w.xAxis, w.xAxis.navigatorAxis.toFixedRange = E.AdditionsClass.prototype.toFixedRange.bind(w.xAxis.navigatorAxis)); f.options.scrollbar.enabled && (f.scrollbar = w.scrollbar = new x(f.renderer, I(f.options.scrollbar, {
                            margin: w.navigatorEnabled ?
                                0 : 10, vertical: f.inverted
                        }), f), v(w.scrollbar, "changed", function (b) { var a = w.size, y = a * this.to; a *= this.from; w.hasDragged = w.scrollbar.hasDragged; w.render(0, 0, a, y); (f.options.scrollbar.liveRedraw || "mousemove" !== b.DOMType && "touchmove" !== b.DOMType) && setTimeout(function () { w.onMouseUp(b) }) })); w.addBaseSeriesEvents(); w.addChartEvents()
                }; b.prototype.getUnionExtremes = function (f) {
                    var b = this.chart.xAxis[0], a = this.xAxis, d = a.options, p = b.options, g; f && null === b.dataMin || (g = {
                        dataMin: D(d && d.min, r("min", p.min, b.dataMin,
                            a.dataMin, a.min)), dataMax: D(d && d.max, r("max", p.max, b.dataMax, a.dataMax, a.max))
                    }); return g
                }; b.prototype.setBaseSeries = function (f, b) {
                    var d = this.chart, y = this.baseSeries = []; f = f || d.options && d.options.navigator.baseSeries || (d.series.length ? a(d.series, function (f) { return !f.options.isInternal }).index : 0); (d.series || []).forEach(function (b, a) { b.options.isInternal || !b.options.showInNavigator && (a !== f && b.options.id !== f || !1 === b.options.showInNavigator) || y.push(b) }); this.xAxis && !this.xAxis.navigatorAxis.fake && this.updateNavigatorSeries(!0,
                        b)
                }; b.prototype.updateNavigatorSeries = function (b, a) {
                    var f = this, d = f.chart, y = f.baseSeries, p, g, r = f.navigatorOptions.series, w, e = { enableMouseTracking: !1, index: null, linkedTo: null, group: "nav", padXAxis: !1, xAxis: "navigator-x-axis", yAxis: "navigator-y-axis", showInLegend: !1, stacking: void 0, isInternal: !0, states: { inactive: { opacity: 1 } } }, z = f.series = (f.series || []).filter(function (b) {
                        var a = b.baseSeries; return 0 > y.indexOf(a) ? (a && (m(a, "updatedData", f.updatedDataHandler), delete a.navigatorSeries), b.chart && b.destroy(),
                            !1) : !0
                    }); y && y.length && y.forEach(function (b) {
                        var k = b.navigatorSeries, m = n({ color: b.color, visible: b.visible }, h(r) ? c.navigator.series : r); k && !1 === f.navigatorOptions.adaptToUpdatedData || (e.name = "Navigator " + y.length, p = b.options || {}, w = p.navigatorOptions || {}, g = I(p, e, m, w), g.pointRange = D(m.pointRange, w.pointRange, c.plotOptions[g.type || "line"].pointRange), m = w.data || m.data, f.hasNavigatorData = f.hasNavigatorData || !!m, g.data = m || p.data && p.data.slice(0), k && k.options ? k.update(g, a) : (b.navigatorSeries = d.initSeries(g),
                            b.navigatorSeries.baseSeries = b, z.push(b.navigatorSeries)))
                    }); if (r.data && (!y || !y.length) || h(r)) f.hasNavigatorData = !1, r = k(r), r.forEach(function (b, a) { e.name = "Navigator " + (z.length + 1); g = I(c.navigator.series, { color: d.series[a] && !d.series[a].options.isInternal && d.series[a].color || d.options.colors[a] || d.options.colors[0] }, e, b); g.data = b.data; g.data && (f.hasNavigatorData = !0, z.push(d.initSeries(g))) }); b && this.addBaseSeriesEvents()
                }; b.prototype.addBaseSeriesEvents = function () {
                    var b = this, a = b.baseSeries || []; a[0] &&
                        a[0].xAxis && v(a[0].xAxis, "foundExtremes", this.modifyBaseAxisExtremes); a.forEach(function (f) {
                            v(f, "show", function () { this.navigatorSeries && this.navigatorSeries.setVisible(!0, !1) }); v(f, "hide", function () { this.navigatorSeries && this.navigatorSeries.setVisible(!1, !1) }); !1 !== this.navigatorOptions.adaptToUpdatedData && f.xAxis && v(f, "updatedData", this.updatedDataHandler); v(f, "remove", function () {
                                this.navigatorSeries && (A(b.series, this.navigatorSeries), C(this.navigatorSeries.options) && this.navigatorSeries.remove(!1),
                                    delete this.navigatorSeries)
                            })
                        }, this)
                }; b.prototype.getBaseSeriesMin = function (b) { return this.baseSeries.reduce(function (b, f) { return Math.min(b, f.xData ? f.xData[0] : b) }, b) }; b.prototype.modifyNavigatorAxisExtremes = function () { var b = this.xAxis, a; "undefined" !== typeof b.getExtremes && (!(a = this.getUnionExtremes(!0)) || a.dataMin === b.min && a.dataMax === b.max || (b.min = a.dataMin, b.max = a.dataMax)) }; b.prototype.modifyBaseAxisExtremes = function () {
                    var b = this.chart.navigator, a = this.getExtremes(), d = a.dataMin, p = a.dataMax; a =
                        a.max - a.min; var r = b.stickToMin, w = b.stickToMax, k = D(this.options.overscroll, 0), h = b.series && b.series[0], c = !!this.setExtremes; if (!this.eventArgs || "rangeSelectorButton" !== this.eventArgs.trigger) { if (r) { var m = d; var n = m + a } w && (n = p + k, r || (m = Math.max(d, n - a, b.getBaseSeriesMin(h && h.xData ? h.xData[0] : -Number.MAX_VALUE)))); c && (r || w) && g(m) && (this.min = this.userMin = m, this.max = this.userMax = n) } b.stickToMin = b.stickToMax = null
                }; b.prototype.updatedDataHandler = function () {
                    var b = this.chart.navigator, a = this.navigatorSeries, d =
                        b.getBaseSeriesMin(this.xData[0]); b.stickToMax = b.reversedExtremes ? 0 === Math.round(b.zoomedMin) : Math.round(b.zoomedMax) >= Math.round(b.size); b.stickToMin = g(this.xAxis.min) && this.xAxis.min <= d && (!this.chart.fixedRange || !b.stickToMax); a && !b.hasNavigatorData && (a.options.pointStart = this.xData[0], a.setData(this.options.data, !1, null, !1))
                }; b.prototype.addChartEvents = function () {
                    this.eventsToUnbind || (this.eventsToUnbind = []); this.eventsToUnbind.push(v(this.chart, "redraw", function () {
                        var b = this.navigator, a = b && (b.baseSeries &&
                            b.baseSeries[0] && b.baseSeries[0].xAxis || this.xAxis[0]); a && b.render(a.min, a.max)
                    }), v(this.chart, "getMargins", function () { var b = this.navigator, a = b.opposite ? "plotTop" : "marginBottom"; this.inverted && (a = b.opposite ? "marginRight" : "plotLeft"); this[a] = (this[a] || 0) + (b.navigatorEnabled || !this.inverted ? b.outlineHeight : 0) + b.navigatorOptions.margin }))
                }; b.prototype.destroy = function () {
                    this.removeEvents(); this.xAxis && (A(this.chart.xAxis, this.xAxis), A(this.chart.axes, this.xAxis)); this.yAxis && (A(this.chart.yAxis, this.yAxis),
                        A(this.chart.axes, this.yAxis)); (this.series || []).forEach(function (b) { b.destroy && b.destroy() }); "series xAxis yAxis shades outline scrollbarTrack scrollbarRifles scrollbarGroup scrollbar navigatorGroup rendered".split(" ").forEach(function (b) { this[b] && this[b].destroy && this[b].destroy(); this[b] = null }, this);[this.handles].forEach(function (b) { z(b) }, this)
                }; return b
        }(); t.Navigator || (t.Navigator = w, E.compose(l), v(u, "beforeShowResetZoom", function () {
            var b = this.options, f = b.navigator, a = b.rangeSelector; if ((f &&
                f.enabled || a && a.enabled) && (!p && "x" === b.chart.zoomType || p && "x" === b.chart.pinchType)) return !1
        }), v(u, "beforeRender", function () { var b = this.options; if (b.navigator.enabled || b.scrollbar.enabled) this.scroller = this.navigator = new w(this) }), v(u, "afterSetChartSize", function () {
            var b = this.legend, f = this.navigator; if (f) {
                var a = b && b.options; var d = f.xAxis; var p = f.yAxis; var g = f.scrollbarHeight; this.inverted ? (f.left = f.opposite ? this.chartWidth - g - f.height : this.spacing[3] + g, f.top = this.plotTop + g) : (f.left = this.plotLeft + g,
                    f.top = f.navigatorOptions.top || this.chartHeight - f.height - g - this.spacing[2] - (this.rangeSelector && this.extraBottomMargin ? this.rangeSelector.getHeight() : 0) - (a && "bottom" === a.verticalAlign && "proximate" !== a.layout && a.enabled && !a.floating ? b.legendHeight + D(a.margin, 10) : 0) - (this.titleOffset ? this.titleOffset[2] : 0)); d && p && (this.inverted ? d.options.left = p.options.left = f.left : d.options.top = p.options.top = f.top, d.setAxisSize(), p.setAxisSize())
            }
        }), v(u, "update", function (b) {
            var f = b.options.navigator || {}, a = b.options.scrollbar ||
                {}; this.navigator || this.scroller || !f.enabled && !a.enabled || (I(!0, this.options.navigator, f), I(!0, this.options.scrollbar, a), delete b.options.navigator, delete b.options.scrollbar)
        }), v(u, "afterUpdate", function (b) { this.navigator || this.scroller || !this.options.navigator.enabled && !this.options.scrollbar.enabled || (this.scroller = this.navigator = new w(this), D(b.redraw, !0) && this.redraw(b.animation)) }), v(u, "afterAddSeries", function () { this.navigator && this.navigator.setBaseSeries(null, !1) }), v(e, "afterUpdate", function () {
            this.chart.navigator &&
            !this.options.isInternal && this.chart.navigator.setBaseSeries(null, !1)
        }), u.prototype.callbacks.push(function (b) { var f = b.navigator; f && b.xAxis[0] && (b = b.xAxis[0].getExtremes(), f.render(b.min, b.max)) })); t.Navigator = w; return t.Navigator
    }); K(l, "parts/OrdinalAxis.js", [l["parts/Axis.js"], l["parts/Globals.js"], l["parts/Utilities.js"]], function (l, u, B) {
        var t = B.addEvent, E = B.css, e = B.defined, x = B.pick, q = B.timeUnits; B = u.Chart; var c = u.Series, v; (function (c) {
            var l = function () {
                function c(c) { this.index = {}; this.axis = c } c.prototype.beforeSetTickPositions =
                    function () {
                        var c = this.axis, e = c.ordinal, n = [], a, h = !1, g = c.getExtremes(), q = g.min, D = g.max, m, k = c.isXAxis && !!c.options.breaks; g = c.options.ordinal; var d = Number.MAX_VALUE, p = c.chart.options.chart.ignoreHiddenSeries, r; if (g || k) {
                            c.series.forEach(function (b, g) {
                                a = []; if (!(p && !1 === b.visible || !1 === b.takeOrdinalPosition && !k) && (n = n.concat(b.processedXData), w = n.length, n.sort(function (b, f) { return b - f }), d = Math.min(d, x(b.closestPointRange, d)), w)) {
                                    for (g = 0; g < w - 1;)n[g] !== n[g + 1] && a.push(n[g + 1]), g++; a[0] !== n[0] && a.unshift(n[0]);
                                    n = a
                                } b.isSeriesBoosting && (r = !0)
                            }); r && (n.length = 0); var w = n.length; if (2 < w) { var b = n[1] - n[0]; for (m = w - 1; m-- && !h;)n[m + 1] - n[m] !== b && (h = !0); !c.options.keepOrdinalPadding && (n[0] - q > b || D - n[n.length - 1] > b) && (h = !0) } else c.options.overscroll && (2 === w ? d = n[1] - n[0] : 1 === w ? (d = c.options.overscroll, n = [n[0], n[0] + d]) : d = e.overscrollPointsRange); h ? (c.options.overscroll && (e.overscrollPointsRange = d, n = n.concat(e.getOverscrollPositions())), e.positions = n, b = c.ordinal2lin(Math.max(q, n[0]), !0), m = Math.max(c.ordinal2lin(Math.min(D, n[n.length -
                                1]), !0), 1), e.slope = D = (D - q) / (m - b), e.offset = q - b * D) : (e.overscrollPointsRange = x(c.closestPointRange, e.overscrollPointsRange), e.positions = c.ordinal.slope = e.offset = void 0)
                        } c.isOrdinal = g && h; e.groupIntervalFactor = null
                    }; c.prototype.getExtendedPositions = function () {
                        var c = this, e = c.axis, n = e.constructor.prototype, a = e.chart, h = e.series[0].currentDataGrouping, g = c.index, q = h ? h.count + h.unitName : "raw", D = e.options.overscroll, m = e.getExtremes(), k; g || (g = c.index = {}); if (!g[q]) {
                            var d = {
                                series: [], chart: a, getExtremes: function () {
                                    return {
                                        min: m.dataMin,
                                        max: m.dataMax + D
                                    }
                                }, options: { ordinal: !0 }, ordinal: {}, ordinal2lin: n.ordinal2lin, val2lin: n.val2lin
                            }; d.ordinal.axis = d; e.series.forEach(function (g) { k = { xAxis: d, xData: g.xData.slice(), chart: a, destroyGroupedData: u.noop, getProcessedData: u.Series.prototype.getProcessedData }; k.xData = k.xData.concat(c.getOverscrollPositions()); k.options = { dataGrouping: h ? { enabled: !0, forced: !0, approximation: "open", units: [[h.unitName, [h.count]]] } : { enabled: !1 } }; g.processData.apply(k); d.series.push(k) }); e.ordinal.beforeSetTickPositions.apply({ axis: d });
                            g[q] = d.ordinal.positions
                        } return g[q]
                    }; c.prototype.getGroupIntervalFactor = function (c, e, n) { n = n.processedXData; var a = n.length, h = []; var g = this.groupIntervalFactor; if (!g) { for (g = 0; g < a - 1; g++)h[g] = n[g + 1] - n[g]; h.sort(function (a, g) { return a - g }); h = h[Math.floor(a / 2)]; c = Math.max(c, n[0]); e = Math.min(e, n[a - 1]); this.groupIntervalFactor = g = a * h / (e - c) } return g }; c.prototype.getOverscrollPositions = function () {
                        var c = this.axis, q = c.options.overscroll, n = this.overscrollPointsRange, a = [], h = c.dataMax; if (e(n)) for (a.push(h); h <= c.dataMax +
                            q;)h += n, a.push(h); return a
                    }; c.prototype.postProcessTickInterval = function (c) { var e = this.axis, n = this.slope; return n ? e.options.breaks ? e.closestPointRange || c : c / (n / e.closestPointRange) : c }; return c
            }(); c.Composition = l; c.compose = function (l, z, v) {
                l.keepProps.push("ordinal"); var n = l.prototype; l.prototype.getTimeTicks = function (a, c, g, n, l, m, k) {
                    void 0 === l && (l = []); void 0 === m && (m = 0); var d = 0, p, r, w = {}, b = [], f = -Number.MAX_VALUE, y = this.options.tickPixelInterval, h = this.chart.time, F = []; if (!this.options.ordinal && !this.options.breaks ||
                        !l || 3 > l.length || "undefined" === typeof c) return h.getTimeTicks.apply(h, arguments); var I = l.length; for (p = 0; p < I; p++) { var z = p && l[p - 1] > g; l[p] < c && (d = p); if (p === I - 1 || l[p + 1] - l[p] > 5 * m || z) { if (l[p] > f) { for (r = h.getTimeTicks(a, l[d], l[p], n); r.length && r[0] <= f;)r.shift(); r.length && (f = r[r.length - 1]); F.push(b.length); b = b.concat(r) } d = p + 1 } if (z) break } r = r.info; if (k && r.unitRange <= q.hour) {
                            p = b.length - 1; for (d = 1; d < p; d++)if (h.dateFormat("%d", b[d]) !== h.dateFormat("%d", b[d - 1])) { w[b[d]] = "day"; var v = !0 } v && (w[b[0]] = "day"); r.higherRanks =
                                w
                        } r.segmentStarts = F; b.info = r; if (k && e(y)) { d = F = b.length; v = []; var D; for (h = []; d--;)p = this.translate(b[d]), D && (h[d] = D - p), v[d] = D = p; h.sort(); h = h[Math.floor(h.length / 2)]; h < .6 * y && (h = null); d = b[F - 1] > g ? F - 1 : F; for (D = void 0; d--;)p = v[d], F = Math.abs(D - p), D && F < .8 * y && (null === h || F < .8 * h) ? (w[b[d]] && !w[b[d + 1]] ? (F = d + 1, D = p) : F = d, b.splice(F, 1)) : D = p } return b
                }; n.lin2val = function (a, c) {
                    var g = this.ordinal, h = g.positions; if (h) {
                        var e = g.slope, m = g.offset; g = h.length - 1; if (c) if (0 > a) a = h[0]; else if (a > g) a = h[g]; else {
                            g = Math.floor(a); var k = a -
                                g
                        } else for (; g--;)if (c = e * g + m, a >= c) { e = e * (g + 1) + m; k = (a - c) / (e - c); break } return "undefined" !== typeof k && "undefined" !== typeof h[g] ? h[g] + (k ? k * (h[g + 1] - h[g]) : 0) : a
                    } return a
                }; n.val2lin = function (a, c) { var g = this.ordinal, h = g.positions; if (h) { var e = h.length, m; for (m = e; m--;)if (h[m] === a) { var k = m; break } for (m = e - 1; m--;)if (a > h[m] || 0 === m) { a = (a - h[m]) / (h[m + 1] - h[m]); k = m + a; break } c = c ? k : g.slope * (k || 0) + g.offset } else c = a; return c }; n.ordinal2lin = n.val2lin; t(l, "afterInit", function () { this.ordinal || (this.ordinal = new c.Composition(this)) });
                t(l, "foundExtremes", function () { this.isXAxis && e(this.options.overscroll) && this.max === this.dataMax && (!this.chart.mouseIsDown || this.isInternal) && (!this.eventArgs || this.eventArgs && "navigator" !== this.eventArgs.trigger) && (this.max += this.options.overscroll, !this.isInternal && e(this.userMin) && (this.min += this.options.overscroll)) }); t(l, "afterSetScale", function () { this.horiz && !this.isDirty && (this.isDirty = this.isOrdinal && this.chart.navigator && !this.chart.navigator.adaptToUpdatedData) }); t(l, "initialAxisTranslation",
                    function () { this.ordinal && (this.ordinal.beforeSetTickPositions(), this.tickInterval = this.ordinal.postProcessTickInterval(this.tickInterval)) }); t(z, "pan", function (a) {
                        var c = this.xAxis[0], g = c.options.overscroll, e = a.originalEvent.chartX, n = this.options.chart && this.options.chart.panning, m = !1; if (n && "y" !== n.type && c.options.ordinal && c.series.length) {
                            var k = this.mouseDownX, d = c.getExtremes(), p = d.dataMax, r = d.min, w = d.max, b = this.hoverPoints, f = c.closestPointRange || c.ordinal && c.ordinal.overscrollPointsRange; k = (k - e) /
                                (c.translationSlope * (c.ordinal.slope || f)); var y = { ordinal: { positions: c.ordinal.getExtendedPositions() } }; f = c.lin2val; var q = c.val2lin; if (!y.ordinal.positions) m = !0; else if (1 < Math.abs(k)) {
                                    b && b.forEach(function (b) { b.setState() }); if (0 > k) { b = y; var F = c.ordinal.positions ? c : y } else b = c.ordinal.positions ? c : y, F = y; y = F.ordinal.positions; p > y[y.length - 1] && y.push(p); this.fixedRange = w - r; k = c.navigatorAxis.toFixedRange(null, null, f.apply(b, [q.apply(b, [r, !0]) + k, !0]), f.apply(F, [q.apply(F, [w, !0]) + k, !0])); k.min >= Math.min(d.dataMin,
                                        r) && k.max <= Math.max(p, w) + g && c.setExtremes(k.min, k.max, !0, !1, { trigger: "pan" }); this.mouseDownX = e; E(this.container, { cursor: "move" })
                                }
                        } else m = !0; m || n && /y/.test(n.type) ? g && (c.max = c.dataMax + g) : a.preventDefault()
                    }); t(v, "updatedData", function () { var a = this.xAxis; a && a.options.ordinal && delete a.ordinal.index })
            }
        })(v || (v = {})); v.compose(l, B, c); return v
    }); K(l, "modules/broken-axis.src.js", [l["parts/Axis.js"], l["parts/Globals.js"], l["parts/Utilities.js"], l["parts/Stacking.js"]], function (l, u, B, t) {
        var E = B.addEvent, e =
            B.find, x = B.fireEvent, q = B.isArray, c = B.isNumber, v = B.pick, J = u.Series, G = function () {
                function c(c) { this.hasBreaks = !1; this.axis = c } c.isInBreak = function (c, e) { var n = c.repeat || Infinity, a = c.from, h = c.to - c.from; e = e >= a ? (e - a) % n : n - (a - e) % n; return c.inclusive ? e <= h : e < h && 0 !== e }; c.lin2Val = function (e) { var q = this.brokenAxis; q = q && q.breakArray; if (!q) return e; var n; for (n = 0; n < q.length; n++) { var a = q[n]; if (a.from >= e) break; else a.to < e ? e += a.len : c.isInBreak(a, e) && (e += a.len) } return e }; c.val2Lin = function (e) {
                    var q = this.brokenAxis; q =
                        q && q.breakArray; if (!q) return e; var n = e, a; for (a = 0; a < q.length; a++) { var h = q[a]; if (h.to <= e) n -= h.len; else if (h.from >= e) break; else if (c.isInBreak(h, e)) { n -= e - h.from; break } } return n
                }; c.prototype.findBreakAt = function (c, q) { return e(q, function (e) { return e.from < c && c < e.to }) }; c.prototype.isInAnyBreak = function (e, q) { var n = this.axis, a = n.options.breaks, h = a && a.length, g; if (h) { for (; h--;)if (c.isInBreak(a[h], e)) { var l = !0; g || (g = v(a[h].showPoints, !n.isXAxis)) } var D = l && q ? l && !g : l } return D }; c.prototype.setBreaks = function (e,
                    t) {
                        var n = this, a = n.axis, h = q(e) && !!e.length; a.isDirty = n.hasBreaks !== h; n.hasBreaks = h; a.options.breaks = a.userOptions.breaks = e; a.forceRedraw = !0; a.series.forEach(function (a) { a.isDirty = !0 }); h || a.val2lin !== c.val2Lin || (delete a.val2lin, delete a.lin2val); h && (a.userOptions.ordinal = !1, a.lin2val = c.lin2Val, a.val2lin = c.val2Lin, a.setExtremes = function (a, c, e, h, k) {
                            if (n.hasBreaks) { for (var d, g = this.options.breaks; d = n.findBreakAt(a, g);)a = d.to; for (; d = n.findBreakAt(c, g);)c = d.from; c < a && (c = a) } l.prototype.setExtremes.call(this,
                                a, c, e, h, k)
                        }, a.setAxisTranslation = function (g) {
                            l.prototype.setAxisTranslation.call(this, g); n.unitLength = null; if (n.hasBreaks) {
                                g = a.options.breaks || []; var e = [], h = [], m = 0, k, d = a.userMin || a.min, p = a.userMax || a.max, r = v(a.pointRangePadding, 0), w; g.forEach(function (b) { k = b.repeat || Infinity; c.isInBreak(b, d) && (d += b.to % k - d % k); c.isInBreak(b, p) && (p -= p % k - b.from % k) }); g.forEach(function (b) {
                                    f = b.from; for (k = b.repeat || Infinity; f - k > d;)f -= k; for (; f < d;)f += k; for (w = f; w < p; w += k)e.push({ value: w, move: "in" }), e.push({
                                        value: w + (b.to - b.from),
                                        move: "out", size: b.breakSize
                                    })
                                }); e.sort(function (b, f) { return b.value === f.value ? ("in" === b.move ? 0 : 1) - ("in" === f.move ? 0 : 1) : b.value - f.value }); var b = 0; var f = d; e.forEach(function (a) { b += "in" === a.move ? 1 : -1; 1 === b && "in" === a.move && (f = a.value); 0 === b && (h.push({ from: f, to: a.value, len: a.value - f - (a.size || 0) }), m += a.value - f - (a.size || 0)) }); a.breakArray = n.breakArray = h; n.unitLength = p - d - m + r; x(a, "afterBreaks"); a.staticScale ? a.transA = a.staticScale : n.unitLength && (a.transA *= (p - a.min + r) / n.unitLength); r && (a.minPixelPadding = a.transA *
                                    a.minPointOffset); a.min = d; a.max = p
                            }
                        }); v(t, !0) && a.chart.redraw()
                }; return c
            }(); u = function () {
                function e() { } e.compose = function (e, q) {
                    e.keepProps.push("brokenAxis"); var n = J.prototype; n.drawBreaks = function (a, e) {
                        var g = this, h = g.points, n, m, k, d; if (a && a.brokenAxis && a.brokenAxis.hasBreaks) {
                            var p = a.brokenAxis; e.forEach(function (r) {
                                n = p && p.breakArray || []; m = a.isXAxis ? a.min : v(g.options.threshold, a.min); h.forEach(function (g) {
                                    d = v(g["stack" + r.toUpperCase()], g[r]); n.forEach(function (b) {
                                        if (c(m) && c(d)) {
                                            k = !1; if (m < b.from &&
                                                d > b.to || m > b.from && d < b.from) k = "pointBreak"; else if (m < b.from && d > b.from && d < b.to || m > b.from && d > b.to && d < b.from) k = "pointInBreak"; k && x(a, k, { point: g, brk: b })
                                        }
                                    })
                                })
                            })
                        }
                    }; n.gappedPath = function () {
                        var a = this.currentDataGrouping, c = a && a.gapSize; a = this.options.gapSize; var g = this.points.slice(), e = g.length - 1, n = this.yAxis, m; if (a && 0 < e) for ("value" !== this.options.gapUnit && (a *= this.basePointRange), c && c > a && c >= this.basePointRange && (a = c), m = void 0; e--;)m && !1 !== m.visible || (m = g[e + 1]), c = g[e], !1 !== m.visible && !1 !== c.visible && (m.x -
                            c.x > a && (m = (c.x + m.x) / 2, g.splice(e + 1, 0, { isNull: !0, x: m }), n.stacking && this.options.stacking && (m = n.stacking.stacks[this.stackKey][m] = new t(n, n.options.stackLabels, !1, m, this.stack), m.total = 0)), m = c); return this.getGraphPath(g)
                    }; E(e, "init", function () { this.brokenAxis || (this.brokenAxis = new G(this)) }); E(e, "afterInit", function () { "undefined" !== typeof this.brokenAxis && this.brokenAxis.setBreaks(this.options.breaks, !1) }); E(e, "afterSetTickPositions", function () {
                        var a = this.brokenAxis; if (a && a.hasBreaks) {
                            var c = this.tickPositions,
                            g = this.tickPositions.info, e = [], n; for (n = 0; n < c.length; n++)a.isInAnyBreak(c[n]) || e.push(c[n]); this.tickPositions = e; this.tickPositions.info = g
                        }
                    }); E(e, "afterSetOptions", function () { this.brokenAxis && this.brokenAxis.hasBreaks && (this.options.ordinal = !1) }); E(q, "afterGeneratePoints", function () {
                        var a = this.options.connectNulls, c = this.points, g = this.xAxis, e = this.yAxis; if (this.isDirty) for (var n = c.length; n--;) {
                            var m = c[n], k = !(null === m.y && !1 === a) && (g && g.brokenAxis && g.brokenAxis.isInAnyBreak(m.x, !0) || e && e.brokenAxis &&
                                e.brokenAxis.isInAnyBreak(m.y, !0)); m.visible = k ? !1 : !1 !== m.options.visible
                        }
                    }); E(q, "afterRender", function () { this.drawBreaks(this.xAxis, ["x"]); this.drawBreaks(this.yAxis, v(this.pointArrayMap, ["y"])) })
                }; return e
            }(); u.compose(l, J); return u
    }); K(l, "masters/modules/broken-axis.src.js", [], function () { }); K(l, "parts/DataGrouping.js", [l["parts/DateTimeAxis.js"], l["parts/Globals.js"], l["parts/Options.js"], l["parts/Point.js"], l["parts/Tooltip.js"], l["parts/Utilities.js"]], function (l, u, B, t, E, e) {
        ""; var x = e.addEvent,
            q = e.arrayMax, c = e.arrayMin, v = e.correctFloat, J = e.defined, G = e.error, C = e.extend, z = e.format, A = e.isNumber, n = e.merge, a = e.pick, h = u.Axis; e = u.Series; var g = u.approximations = {
                sum: function (b) { var a = b.length; if (!a && b.hasNulls) var d = null; else if (a) for (d = 0; a--;)d += b[a]; return d }, average: function (b) { var a = b.length; b = g.sum(b); A(b) && a && (b = v(b / a)); return b }, averages: function () { var b = [];[].forEach.call(arguments, function (a) { b.push(g.average(a)) }); return "undefined" === typeof b[0] ? void 0 : b }, open: function (b) {
                    return b.length ?
                        b[0] : b.hasNulls ? null : void 0
                }, high: function (b) { return b.length ? q(b) : b.hasNulls ? null : void 0 }, low: function (b) { return b.length ? c(b) : b.hasNulls ? null : void 0 }, close: function (b) { return b.length ? b[b.length - 1] : b.hasNulls ? null : void 0 }, ohlc: function (b, a, d, c) { b = g.open(b); a = g.high(a); d = g.low(d); c = g.close(c); if (A(b) || A(a) || A(d) || A(c)) return [b, a, d, c] }, range: function (b, a) { b = g.low(b); a = g.high(a); if (A(b) || A(a)) return [b, a]; if (null === b && null === a) return null }
            }, I = function (b, a, d, c) {
                var f = this, p = f.data, r = f.options && f.options.data,
                e = [], w = [], k = [], y = b.length, h = !!a, m = [], q = f.pointArrayMap, l = q && q.length, H = ["x"].concat(q || ["y"]), v = 0, t = 0, x; c = "function" === typeof c ? c : g[c] ? g[c] : g[f.getDGApproximation && f.getDGApproximation() || "average"]; l ? q.forEach(function () { m.push([]) }) : m.push([]); var D = l || 1; for (x = 0; x <= y && !(b[x] >= d[0]); x++); for (x; x <= y; x++) {
                    for (; "undefined" !== typeof d[v + 1] && b[x] >= d[v + 1] || x === y;) {
                        var u = d[v]; f.dataGroupInfo = { start: f.cropStart + t, length: m[0].length }; var z = c.apply(f, m); f.pointClass && !J(f.dataGroupInfo.options) && (f.dataGroupInfo.options =
                            n(f.pointClass.prototype.optionsToObject.call({ series: f }, f.options.data[f.cropStart + t])), H.forEach(function (b) { delete f.dataGroupInfo.options[b] })); "undefined" !== typeof z && (e.push(u), w.push(z), k.push(f.dataGroupInfo)); t = x; for (u = 0; u < D; u++)m[u].length = 0, m[u].hasNulls = !1; v += 1; if (x === y) break
                    } if (x === y) break; if (q) for (u = f.cropStart + x, z = p && p[u] || f.pointClass.prototype.applyOptions.apply({ series: f }, [r[u]]), u = 0; u < l; u++) { var E = z[q[u]]; A(E) ? m[u].push(E) : null === E && (m[u].hasNulls = !0) } else u = h ? a[x] : null, A(u) ? m[0].push(u) :
                        null === u && (m[0].hasNulls = !0)
                } return { groupedXData: e, groupedYData: w, groupMap: k }
            }, D = { approximations: g, groupData: I }, m = e.prototype, k = m.processData, d = m.generatePoints, p = {
                groupPixelWidth: 2, dateTimeLabelFormats: {
                    millisecond: ["%A, %b %e, %H:%M:%S.%L", "%A, %b %e, %H:%M:%S.%L", "-%H:%M:%S.%L"], second: ["%A, %b %e, %H:%M:%S", "%A, %b %e, %H:%M:%S", "-%H:%M:%S"], minute: ["%A, %b %e, %H:%M", "%A, %b %e, %H:%M", "-%H:%M"], hour: ["%A, %b %e, %H:%M", "%A, %b %e, %H:%M", "-%H:%M"], day: ["%A, %b %e, %Y", "%A, %b %e", "-%A, %b %e, %Y"],
                    week: ["Week from %A, %b %e, %Y", "%A, %b %e", "-%A, %b %e, %Y"], month: ["%B %Y", "%B", "-%B %Y"], year: ["%Y", "%Y", "-%Y"]
                }
            }, r = { line: {}, spline: {}, area: {}, areaspline: {}, arearange: {}, column: { groupPixelWidth: 10 }, columnrange: { groupPixelWidth: 10 }, candlestick: { groupPixelWidth: 10 }, ohlc: { groupPixelWidth: 5 } }, w = u.defaultDataGroupingUnits = [["millisecond", [1, 2, 5, 10, 20, 25, 50, 100, 200, 500]], ["second", [1, 2, 5, 10, 15, 30]], ["minute", [1, 2, 5, 10, 15, 30]], ["hour", [1, 2, 3, 4, 6, 8, 12]], ["day", [1]], ["week", [1]], ["month", [1, 3, 6]], ["year",
                null]]; m.getDGApproximation = function () { return this.is("arearange") ? "range" : this.is("ohlc") ? "ohlc" : this.is("column") ? "sum" : "average" }; m.groupData = I; m.processData = function () {
                    var b = this.chart, f = this.options.dataGrouping, d = !1 !== this.allowDG && f && a(f.enabled, b.options.isStock), c = this.visible || !b.options.chart.ignoreHiddenSeries, g, p = this.currentDataGrouping, r = !1; this.forceCrop = d; this.groupPixelWidth = null; this.hasProcessed = !0; d && !this.requireSorting && (this.requireSorting = r = !0); d = !1 === k.apply(this, arguments) ||
                        !d; r && (this.requireSorting = !1); if (!d) {
                            this.destroyGroupedData(); d = f.groupAll ? this.xData : this.processedXData; var e = f.groupAll ? this.yData : this.processedYData, h = b.plotSizeX; b = this.xAxis; var n = b.options.ordinal, q = this.groupPixelWidth = b.getGroupPixelWidth && b.getGroupPixelWidth(); if (q) {
                                this.isDirty = g = !0; this.points = null; r = b.getExtremes(); var v = r.min; r = r.max; n = n && b.ordinal && b.ordinal.getGroupIntervalFactor(v, r, this) || 1; q = q * (r - v) / h * n; h = b.getTimeTicks(l.AdditionsClass.prototype.normalizeTimeTickInterval(q,
                                    f.units || w), Math.min(v, d[0]), Math.max(r, d[d.length - 1]), b.options.startOfWeek, d, this.closestPointRange); e = m.groupData.apply(this, [d, e, h, f.approximation]); d = e.groupedXData; n = e.groupedYData; var u = 0; if (f.smoothed && d.length) { var x = d.length - 1; for (d[x] = Math.min(d[x], r); x-- && 0 < x;)d[x] += q / 2; d[0] = Math.max(d[0], v) } for (x = 1; x < h.length; x++)h.info.segmentStarts && -1 !== h.info.segmentStarts.indexOf(x) || (u = Math.max(h[x] - h[x - 1], u)); v = h.info; v.gapSize = u; this.closestPointRange = h.info.totalRange; this.groupMap = e.groupMap;
                                if (J(d[0]) && d[0] < b.min && c) { if (!J(b.options.min) && b.min <= b.dataMin || b.min === b.dataMin) b.min = Math.min(d[0], b.min); b.dataMin = Math.min(d[0], b.dataMin) } f.groupAll && (f = this.cropData(d, n, b.min, b.max, 1), d = f.xData, n = f.yData); this.processedXData = d; this.processedYData = n
                            } else this.groupMap = null; this.hasGroupedData = g; this.currentDataGrouping = v; this.preventGraphAnimation = (p && p.totalRange) !== (v && v.totalRange)
                        }
                }; m.destroyGroupedData = function () {
                    this.groupedData && (this.groupedData.forEach(function (b, a) {
                        b && (this.groupedData[a] =
                            b.destroy ? b.destroy() : null)
                    }, this), this.groupedData.length = 0)
                }; m.generatePoints = function () { d.apply(this); this.destroyGroupedData(); this.groupedData = this.hasGroupedData ? this.points : null }; x(t, "update", function () { if (this.dataGroup) return G(24, !1, this.series.chart), !1 }); x(E, "headerFormatter", function (b) {
                    var a = this.chart, d = a.time, c = b.labelConfig, g = c.series, r = g.tooltipOptions, e = g.options.dataGrouping, w = r.xDateFormat, k = g.xAxis, h = r[(b.isFooter ? "footer" : "header") + "Format"]; if (k && "datetime" === k.options.type &&
                        e && A(c.key)) { var n = g.currentDataGrouping; e = e.dateTimeLabelFormats || p.dateTimeLabelFormats; if (n) if (r = e[n.unitName], 1 === n.count) w = r[0]; else { w = r[1]; var m = r[2] } else !w && e && (w = this.getXDateFormat(c, r, k)); w = d.dateFormat(w, c.key); m && (w += d.dateFormat(m, c.key + n.totalRange - 1)); g.chart.styledMode && (h = this.styledModeFormat(h)); b.text = z(h, { point: C(c.point, { key: w }), series: g }, a); b.preventDefault() }
                }); x(e, "destroy", m.destroyGroupedData); x(e, "afterSetOptions", function (b) {
                    b = b.options; var a = this.type, d = this.chart.options.plotOptions,
                        c = B.defaultOptions.plotOptions[a].dataGrouping, g = this.useCommonDataGrouping && p; if (r[a] || g) c || (c = n(p, r[a])), b.dataGrouping = n(g, c, d.series && d.series.dataGrouping, d[a].dataGrouping, this.userOptions.dataGrouping)
                }); x(h, "afterSetScale", function () { this.series.forEach(function (b) { b.hasProcessed = !1 }) }); h.prototype.getGroupPixelWidth = function () {
                    var b = this.series, f = b.length, d, c = 0, g = !1, r; for (d = f; d--;)(r = b[d].options.dataGrouping) && (c = Math.max(c, a(r.groupPixelWidth, p.groupPixelWidth))); for (d = f; d--;)(r = b[d].options.dataGrouping) &&
                        b[d].hasProcessed && (f = (b[d].processedXData || b[d].data).length, b[d].groupPixelWidth || f > this.chart.plotSizeX / c || f && r.forced) && (g = !0); return g ? c : 0
                }; h.prototype.setDataGrouping = function (b, d) { var f; d = a(d, !0); b || (b = { forced: !1, units: null }); if (this instanceof h) for (f = this.series.length; f--;)this.series[f].update({ dataGrouping: b }, !1); else this.chart.options.series.forEach(function (a) { a.dataGrouping = b }, !1); this.ordinal && (this.ordinal.slope = void 0); d && this.chart.redraw() }; u.dataGrouping = D; ""; return D
    }); K(l,
        "parts/OHLCSeries.js", [l["parts/Globals.js"], l["parts/Point.js"], l["parts/Utilities.js"]], function (l, u, B) {
            B = B.seriesType; var t = l.seriesTypes; B("ohlc", "column", { lineWidth: 1, tooltip: { pointFormat: '<span style="color:{point.color}">\u25cf</span> <b> {series.name}</b><br/>Open: {point.open}<br/>High: {point.high}<br/>Low: {point.low}<br/>Close: {point.close}<br/>' }, threshold: null, states: { hover: { lineWidth: 3 } }, stickyTracking: !0 }, {
                directTouch: !1, pointArrayMap: ["open", "high", "low", "close"], toYData: function (l) {
                    return [l.open,
                    l.high, l.low, l.close]
                }, pointValKey: "close", pointAttrToOptions: { stroke: "color", "stroke-width": "lineWidth" }, init: function () { t.column.prototype.init.apply(this, arguments); this.options.stacking = void 0 }, pointAttribs: function (l, e) { e = t.column.prototype.pointAttribs.call(this, l, e); var x = this.options; delete e.fill; !l.options.color && x.upColor && l.open < l.close && (e.stroke = x.upColor); return e }, translate: function () {
                    var l = this, e = l.yAxis, x = !!l.modifyValue, q = ["plotOpen", "plotHigh", "plotLow", "plotClose", "yBottom"];
                    t.column.prototype.translate.apply(l); l.points.forEach(function (c) { [c.open, c.high, c.low, c.close, c.low].forEach(function (v, u) { null !== v && (x && (v = l.modifyValue(v)), c[q[u]] = e.toPixels(v, !0)) }); c.tooltipPos[1] = c.plotHigh + e.pos - l.chart.plotTop })
                }, drawPoints: function () {
                    var l = this, e = l.chart, x = function (e, c, l) { var q = e[0]; e = e[1]; "number" === typeof q[2] && (q[2] = Math.max(l + c, q[2])); "number" === typeof e[2] && (e[2] = Math.min(l - c, e[2])) }; l.points.forEach(function (q) {
                        var c = q.graphic, v = !c; if ("undefined" !== typeof q.plotY) {
                            c ||
                            (q.graphic = c = e.renderer.path().add(l.group)); e.styledMode || c.attr(l.pointAttribs(q, q.selected && "select")); var u = c.strokeWidth(); var t = u % 2 / 2; var C = Math.round(q.plotX) - t; var z = Math.round(q.shapeArgs.width / 2); var A = [["M", C, Math.round(q.yBottom)], ["L", C, Math.round(q.plotHigh)]]; if (null !== q.open) { var n = Math.round(q.plotOpen) + t; A.push(["M", C, n], ["L", C - z, n]); x(A, u / 2, n) } null !== q.close && (n = Math.round(q.plotClose) + t, A.push(["M", C, n], ["L", C + z, n]), x(A, u / 2, n)); c[v ? "attr" : "animate"]({ d: A }).addClass(q.getClassName(),
                                !0)
                        }
                    })
                }, animate: null
            }, { getClassName: function () { return u.prototype.getClassName.call(this) + (this.open < this.close ? " highcharts-point-up" : " highcharts-point-down") } }); ""
        }); K(l, "parts/CandlestickSeries.js", [l["parts/Globals.js"], l["parts/Options.js"], l["parts/Utilities.js"]], function (l, u, B) {
            u = u.defaultOptions; var t = B.merge; B = B.seriesType; var E = l.seriesTypes; B("candlestick", "ohlc", t(u.plotOptions.column, {
                states: { hover: { lineWidth: 2 } }, tooltip: u.plotOptions.ohlc.tooltip, threshold: null, lineColor: "#000000",
                lineWidth: 1, upColor: "#ffffff", stickyTracking: !0
            }), {
                pointAttribs: function (e, l) { var q = E.column.prototype.pointAttribs.call(this, e, l), c = this.options, v = e.open < e.close, u = c.lineColor || this.color; q["stroke-width"] = c.lineWidth; q.fill = e.options.color || (v ? c.upColor || this.color : this.color); q.stroke = e.options.lineColor || (v ? c.upLineColor || u : u); l && (e = c.states[l], q.fill = e.color || q.fill, q.stroke = e.lineColor || q.stroke, q["stroke-width"] = e.lineWidth || q["stroke-width"]); return q }, drawPoints: function () {
                    var e = this, l = e.chart,
                    q = e.yAxis.reversed; e.points.forEach(function (c) {
                        var v = c.graphic, u = !v; if ("undefined" !== typeof c.plotY) {
                            v || (c.graphic = v = l.renderer.path().add(e.group)); e.chart.styledMode || v.attr(e.pointAttribs(c, c.selected && "select")).shadow(e.options.shadow); var x = v.strokeWidth() % 2 / 2; var t = Math.round(c.plotX) - x; var z = c.plotOpen; var A = c.plotClose; var n = Math.min(z, A); z = Math.max(z, A); var a = Math.round(c.shapeArgs.width / 2); A = q ? z !== c.yBottom : Math.round(n) !== Math.round(c.plotHigh); var h = q ? Math.round(n) !== Math.round(c.plotHigh) :
                                z !== c.yBottom; n = Math.round(n) + x; z = Math.round(z) + x; x = []; x.push(["M", t - a, z], ["L", t - a, n], ["L", t + a, n], ["L", t + a, z], ["Z"], ["M", t, n], ["L", t, A ? Math.round(q ? c.yBottom : c.plotHigh) : n], ["M", t, z], ["L", t, h ? Math.round(q ? c.plotHigh : c.yBottom) : z]); v[u ? "attr" : "animate"]({ d: x }).addClass(c.getClassName(), !0)
                        }
                    })
                }
            }); ""
        }); K(l, "mixins/on-series.js", [l["parts/Globals.js"], l["parts/Utilities.js"]], function (l, u) {
            var B = u.defined, t = u.stableSort, E = l.seriesTypes; return {
                getPlotBox: function () {
                    return l.Series.prototype.getPlotBox.call(this.options.onSeries &&
                        this.chart.get(this.options.onSeries) || this)
                }, translate: function () {
                    E.column.prototype.translate.apply(this); var e = this, l = e.options, q = e.chart, c = e.points, v = c.length - 1, u, G = l.onSeries; G = G && q.get(G); l = l.onKey || "y"; var C = G && G.options.step, z = G && G.points, A = z && z.length, n = q.inverted, a = e.xAxis, h = e.yAxis, g = 0, I; if (G && G.visible && A) {
                        g = (G.pointXOffset || 0) + (G.barW || 0) / 2; q = G.currentDataGrouping; var D = z[A - 1].x + (q ? q.totalRange : 0); t(c, function (a, c) { return a.x - c.x }); for (l = "plot" + l[0].toUpperCase() + l.substr(1); A-- && c[v];) {
                            var m =
                                z[A]; q = c[v]; q.y = m.y; if (m.x <= q.x && "undefined" !== typeof m[l]) { if (q.x <= D && (q.plotY = m[l], m.x < q.x && !C && (I = z[A + 1]) && "undefined" !== typeof I[l])) { var k = (q.x - m.x) / (I.x - m.x); q.plotY += k * (I[l] - m[l]); q.y += k * (I.y - m.y) } v--; A++; if (0 > v) break }
                        }
                    } c.forEach(function (d, p) {
                        d.plotX += g; if ("undefined" === typeof d.plotY || n) 0 <= d.plotX && d.plotX <= a.len ? n ? (d.plotY = a.translate(d.x, 0, 1, 0, 1), d.plotX = B(d.y) ? h.translate(d.y, 0, 0, 0, 1) : 0) : d.plotY = (a.opposite ? 0 : e.yAxis.len) + a.offset : d.shapeArgs = {}; if ((u = c[p - 1]) && u.plotX === d.plotX) {
                            "undefined" ===
                            typeof u.stackIndex && (u.stackIndex = 0); var r = u.stackIndex + 1
                        } d.stackIndex = r
                    }); this.onSeries = G
                }
            }
        }); K(l, "parts/FlagsSeries.js", [l["parts/Globals.js"], l["parts/SVGElement.js"], l["parts/SVGRenderer.js"], l["parts/Utilities.js"], l["mixins/on-series.js"]], function (l, u, B, t, E) {
            function e(a) {
                h[a + "pin"] = function (c, g, e, k, d) {
                    var p = d && d.anchorX; d = d && d.anchorY; "circle" === a && k > e && (c -= Math.round((k - e) / 2), e = k); var r = h[a](c, g, e, k); if (p && d) {
                        var w = p; "circle" === a ? w = c + e / 2 : (c = r[0], e = r[1], "M" === c[0] && "L" === e[0] && (w = (c[1] + e[1]) /
                            2)); r.push(["M", w, g > d ? g : g + k], ["L", p, d]); r = r.concat(h.circle(p - 1, d - 1, 2, 2))
                    } return r
                }
            } var x = t.addEvent, q = t.defined, c = t.isNumber, v = t.merge, J = t.objectEach, G = t.seriesType, C = t.wrap; t = l.noop; var z = l.Renderer, A = l.Series, n = l.TrackerMixin, a = l.VMLRenderer, h = B.prototype.symbols; G("flags", "column", {
                pointRange: 0, allowOverlapX: !1, shape: "flag", stackDistance: 12, textAlign: "center", tooltip: { pointFormat: "{point.text}<br/>" }, threshold: null, y: -30, fillColor: "#ffffff", lineWidth: 1, states: { hover: { lineColor: "#000000", fillColor: "#ccd6eb" } },
                style: { fontSize: "11px", fontWeight: "bold" }
            }, {
                sorted: !1, noSharedTooltip: !0, allowDG: !1, takeOrdinalPosition: !1, trackerGroups: ["markerGroup"], forceCrop: !0, init: A.prototype.init, pointAttribs: function (a, c) { var g = this.options, e = a && a.color || this.color, k = g.lineColor, d = a && a.lineWidth; a = a && a.fillColor || g.fillColor; c && (a = g.states[c].fillColor, k = g.states[c].lineColor, d = g.states[c].lineWidth); return { fill: a || e, stroke: k || e, "stroke-width": d || g.lineWidth || 0 } }, translate: E.translate, getPlotBox: E.getPlotBox, drawPoints: function () {
                    var a =
                        this.points, c = this.chart, e = c.renderer, h = c.inverted, k = this.options, d = k.y, p, r = this.yAxis, w = {}, b = []; for (p = a.length; p--;) {
                            var f = a[p]; var n = (h ? f.plotY : f.plotX) > this.xAxis.len; var H = f.plotX; var F = f.stackIndex; var t = f.options.shape || k.shape; var x = f.plotY; "undefined" !== typeof x && (x = f.plotY + d - ("undefined" !== typeof F && F * k.stackDistance)); f.anchorX = F ? void 0 : f.plotX; var z = F ? void 0 : f.plotY; var A = "flag" !== t; F = f.graphic; "undefined" !== typeof x && 0 <= H && !n ? (F || (F = f.graphic = e.label("", null, null, t, null, null, k.useHTML),
                                c.styledMode || F.attr(this.pointAttribs(f)).css(v(k.style, f.style)), F.attr({ align: A ? "center" : "left", width: k.width, height: k.height, "text-align": k.textAlign }).addClass("highcharts-point").add(this.markerGroup), f.graphic.div && (f.graphic.div.point = f), c.styledMode || F.shadow(k.shadow), F.isNew = !0), 0 < H && (H -= F.strokeWidth() % 2), t = { y: x, anchorY: z }, k.allowOverlapX && (t.x = H, t.anchorX = f.anchorX), F.attr({ text: f.options.title || k.title || "A" })[F.isNew ? "attr" : "animate"](t), k.allowOverlapX || (w[f.plotX] ? w[f.plotX].size =
                                    Math.max(w[f.plotX].size, F.width) : w[f.plotX] = { align: A ? .5 : 0, size: F.width, target: H, anchorX: H }), f.tooltipPos = [H, x + r.pos - c.plotTop]) : F && (f.graphic = F.destroy())
                        } k.allowOverlapX || (J(w, function (a) { a.plotX = a.anchorX; b.push(a) }), l.distribute(b, h ? r.len : this.xAxis.len, 100), a.forEach(function (b) { var a = b.graphic && w[b.plotX]; a && (b.graphic[b.graphic.isNew ? "attr" : "animate"]({ x: a.pos + a.align * a.size, anchorX: b.anchorX }), q(a.pos) ? b.graphic.isNew = !1 : (b.graphic.attr({ x: -9999, anchorX: -9999 }), b.graphic.isNew = !0)) })); k.useHTML &&
                            C(this.markerGroup, "on", function (b) { return u.prototype.on.apply(b.apply(this, [].slice.call(arguments, 1)), [].slice.call(arguments, 1)) })
                }, drawTracker: function () { var a = this.points; n.drawTrackerPoint.apply(this); a.forEach(function (c) { var g = c.graphic; g && x(g.element, "mouseover", function () { 0 < c.stackIndex && !c.raised && (c._y = g.y, g.attr({ y: c._y - 8 }), c.raised = !0); a.forEach(function (a) { a !== c && a.raised && a.graphic && (a.graphic.attr({ y: a._y }), a.raised = !1) }) }) }) }, animate: function (a) { a && this.setClip() }, setClip: function () {
                    A.prototype.setClip.apply(this,
                        arguments); !1 !== this.options.clip && this.sharedClipKey && this.markerGroup.clip(this.chart[this.sharedClipKey])
                }, buildKDTree: t, invertGroups: t
            }, { isValid: function () { return c(this.y) || "undefined" === typeof this.y } }); h.flag = function (a, c, e, l, k) { var d = k && k.anchorX || a; k = k && k.anchorY || c; var g = h.circle(d - 1, k - 1, 2, 2); g.push(["M", d, k], ["L", a, c + l], ["L", a, c], ["L", a + e, c], ["L", a + e, c + l], ["L", a, c + l], ["Z"]); return g }; e("circle"); e("square"); z === a && ["circlepin", "flag", "squarepin"].forEach(function (c) {
                a.prototype.symbols[c] =
                h[c]
            }); ""
        }); K(l, "parts/RangeSelector.js", [l["parts/Axis.js"], l["parts/Chart.js"], l["parts/Globals.js"], l["parts/Options.js"], l["parts/SVGElement.js"], l["parts/Utilities.js"]], function (l, u, B, t, E, e) {
            var x = t.defaultOptions, q = e.addEvent, c = e.createElement, v = e.css, J = e.defined, G = e.destroyObjectProperties, C = e.discardElement, z = e.extend, A = e.fireEvent, n = e.isNumber, a = e.merge, h = e.objectEach, g = e.pick, I = e.pInt, D = e.splat; z(x, {
                rangeSelector: {
                    verticalAlign: "top", buttonTheme: { width: 28, height: 18, padding: 2, zIndex: 7 },
                    floating: !1, x: 0, y: 0, height: void 0, inputPosition: { align: "right", x: 0, y: 0 }, buttonPosition: { align: "left", x: 0, y: 0 }, labelStyle: { color: "#666666" }
                }
            }); x.lang = a(x.lang, { rangeSelectorZoom: "Zoom", rangeSelectorFrom: "From", rangeSelectorTo: "To" }); var m = function () {
                function e(a) { this.buttons = void 0; this.buttonOptions = e.prototype.defaultButtons; this.options = void 0; this.chart = a; this.init(a) } e.prototype.clickButton = function (a, c) {
                    var d = this.chart, e = this.buttonOptions[a], b = d.xAxis[0], f = d.scroller && d.scroller.getUnionExtremes() ||
                        b || {}, p = f.dataMin, k = f.dataMax, h = b && Math.round(Math.min(b.max, g(k, b.max))), m = e.type; f = e._range; var v, u = e.dataGrouping; if (null !== p && null !== k) {
                            d.fixedRange = f; u && (this.forcedDataGrouping = !0, l.prototype.setDataGrouping.call(b || { chart: this.chart }, u, !1), this.frozenStates = e.preserveDataGrouping); if ("month" === m || "year" === m) if (b) { m = { range: e, max: h, chart: d, dataMin: p, dataMax: k }; var t = b.minFromRange.call(m); n(m.newMax) && (h = m.newMax) } else f = e; else if (f) t = Math.max(h - f, p), h = Math.min(t + f, k); else if ("ytd" === m) if (b) "undefined" ===
                                typeof k && (p = Number.MAX_VALUE, k = Number.MIN_VALUE, d.series.forEach(function (b) { b = b.xData; p = Math.min(b[0], p); k = Math.max(b[b.length - 1], k) }), c = !1), h = this.getYTDExtremes(k, p, d.time.useUTC), t = v = h.min, h = h.max; else { this.deferredYTDClick = a; return } else "all" === m && b && (t = p, h = k); t += e._offsetMin; h += e._offsetMax; this.setSelected(a); if (b) b.setExtremes(t, h, g(c, 1), null, { trigger: "rangeSelectorButton", rangeSelectorButton: e }); else {
                                    var x = D(d.options.xAxis)[0]; var z = x.range; x.range = f; var A = x.min; x.min = v; q(d, "load", function () {
                                        x.range =
                                        z; x.min = A
                                    })
                                }
                        }
                }; e.prototype.setSelected = function (a) { this.selected = this.options.selected = a }; e.prototype.init = function (a) {
                    var c = this, d = a.options.rangeSelector, e = d.buttons || c.defaultButtons.slice(), b = d.selected, f = function () { var b = c.minInput, a = c.maxInput; b && b.blur && A(b, "blur"); a && a.blur && A(a, "blur") }; c.chart = a; c.options = d; c.buttons = []; c.buttonOptions = e; this.unMouseDown = q(a.container, "mousedown", f); this.unResize = q(a, "resize", f); e.forEach(c.computeButtonRange); "undefined" !== typeof b && e[b] && this.clickButton(b,
                        !1); q(a, "load", function () { a.xAxis && a.xAxis[0] && q(a.xAxis[0], "setExtremes", function (b) { this.max - this.min !== a.fixedRange && "rangeSelectorButton" !== b.trigger && "updatedData" !== b.trigger && c.forcedDataGrouping && !c.frozenStates && this.setDataGrouping(!1, !1) }) })
                }; e.prototype.updateButtonStates = function () {
                    var a = this, c = this.chart, e = c.xAxis[0], g = Math.round(e.max - e.min), b = !e.hasVisibleSeries, f = c.scroller && c.scroller.getUnionExtremes() || e, k = f.dataMin, h = f.dataMax; c = a.getYTDExtremes(h, k, c.time.useUTC); var l = c.min,
                        m = c.max, q = a.selected, v = n(q), u = a.options.allButtonsEnabled, t = a.buttons; a.buttonOptions.forEach(function (c, f) {
                            var d = c._range, p = c.type, r = c.count || 1, w = t[f], n = 0, y = c._offsetMax - c._offsetMin; c = f === q; var x = d > h - k, F = d < e.minRange, z = !1, H = !1; d = d === g; ("month" === p || "year" === p) && g + 36E5 >= 864E5 * { month: 28, year: 365 }[p] * r - y && g - 36E5 <= 864E5 * { month: 31, year: 366 }[p] * r + y ? d = !0 : "ytd" === p ? (d = m - l + y === g, z = !c) : "all" === p && (d = e.max - e.min >= h - k, H = !c && v && d); p = !u && (x || F || H || b); r = c && d || d && !v && !z || c && a.frozenStates; p ? n = 3 : r && (v = !0, n = 2); w.state !==
                                n && (w.setState(n), 0 === n && q === f && a.setSelected(null))
                        })
                }; e.prototype.computeButtonRange = function (a) { var c = a.type, d = a.count || 1, e = { millisecond: 1, second: 1E3, minute: 6E4, hour: 36E5, day: 864E5, week: 6048E5 }; if (e[c]) a._range = e[c] * d; else if ("month" === c || "year" === c) a._range = 864E5 * { month: 30, year: 365 }[c] * d; a._offsetMin = g(a.offsetMin, 0); a._offsetMax = g(a.offsetMax, 0); a._range += a._offsetMax - a._offsetMin }; e.prototype.setInputValue = function (a, c) {
                    var d = this.chart.options.rangeSelector, e = this.chart.time, b = this[a + "Input"];
                    J(c) && (b.previousValue = b.HCTime, b.HCTime = c); b.value = e.dateFormat(d.inputEditDateFormat || "%Y-%m-%d", b.HCTime); this[a + "DateBox"].attr({ text: e.dateFormat(d.inputDateFormat || "%b %e, %Y", b.HCTime) })
                }; e.prototype.showInput = function (a) { var c = this.inputGroup, d = this[a + "DateBox"]; v(this[a + "Input"], { left: c.translateX + d.x + "px", top: c.translateY + "px", width: d.width - 2 + "px", height: d.height - 2 + "px", border: "2px solid silver" }) }; e.prototype.hideInput = function (a) {
                    v(this[a + "Input"], { border: 0, width: "1px", height: "1px" });
                    this.setInputValue(a)
                }; e.prototype.drawInput = function (d) {
                    function e() {
                        var b = m.value, a = (h.inputDateParser || Date.parse)(b), c = k.xAxis[0], f = k.scroller && k.scroller.xAxis ? k.scroller.xAxis : c, d = f.dataMin; f = f.dataMax; a !== m.previousValue && (m.previousValue = a, n(a) || (a = b.split("-"), a = Date.UTC(I(a[0]), I(a[1]) - 1, I(a[2]))), n(a) && (k.time.useUTC || (a += 6E4 * (new Date).getTimezoneOffset()), q ? a > g.maxInput.HCTime ? a = void 0 : a < d && (a = d) : a < g.minInput.HCTime ? a = void 0 : a > f && (a = f), "undefined" !== typeof a && c.setExtremes(q ? a : c.min,
                            q ? c.max : a, void 0, void 0, { trigger: "rangeSelectorInput" })))
                    } var g = this, k = g.chart, b = k.renderer.style || {}, f = k.renderer, h = k.options.rangeSelector, l = g.div, q = "min" === d, m, u, t = this.inputGroup; this[d + "Label"] = u = f.label(x.lang[q ? "rangeSelectorFrom" : "rangeSelectorTo"], this.inputGroup.offset).addClass("highcharts-range-label").attr({ padding: 2 }).add(t); t.offset += u.width + 5; this[d + "DateBox"] = f = f.label("", t.offset).addClass("highcharts-range-input").attr({
                        padding: 2, width: h.inputBoxWidth || 90, height: h.inputBoxHeight ||
                            17, "text-align": "center"
                    }).on("click", function () { g.showInput(d); g[d + "Input"].focus() }); k.styledMode || f.attr({ stroke: h.inputBoxBorderColor || "#cccccc", "stroke-width": 1 }); f.add(t); t.offset += f.width + (q ? 10 : 0); this[d + "Input"] = m = c("input", { name: d, className: "highcharts-range-selector", type: "text" }, { top: k.plotTop + "px" }, l); k.styledMode || (u.css(a(b, h.labelStyle)), f.css(a({ color: "#333333" }, b, h.inputStyle)), v(m, z({
                        position: "absolute", border: 0, width: "1px", height: "1px", padding: 0, textAlign: "center", fontSize: b.fontSize,
                        fontFamily: b.fontFamily, top: "-9999em"
                    }, h.inputStyle))); m.onfocus = function () { g.showInput(d) }; m.onblur = function () { m === B.doc.activeElement && e(); g.hideInput(d); m.blur() }; m.onchange = e; m.onkeypress = function (a) { 13 === a.keyCode && e() }
                }; e.prototype.getPosition = function () { var a = this.chart, c = a.options.rangeSelector; a = "top" === c.verticalAlign ? a.plotTop - a.axisOffset[0] : 0; return { buttonTop: a + c.buttonPosition.y, inputTop: a + c.inputPosition.y - 10 } }; e.prototype.getYTDExtremes = function (a, c, e) {
                    var d = this.chart.time, b = new d.Date(a),
                    f = d.get("FullYear", b); e = e ? d.Date.UTC(f, 0, 1) : +new d.Date(f, 0, 1); c = Math.max(c || 0, e); b = b.getTime(); return { max: Math.min(a || b, b), min: c }
                }; e.prototype.render = function (a, e) {
                    var d = this, p = d.chart, b = p.renderer, f = p.container, k = p.options, h = k.exporting && !1 !== k.exporting.enabled && k.navigation && k.navigation.buttonOptions, l = x.lang, n = d.div, m = k.rangeSelector, q = g(k.chart.style && k.chart.style.zIndex, 0) + 1; k = m.floating; var v = d.buttons; n = d.inputGroup; var u = m.buttonTheme, t = m.buttonPosition, z = m.inputPosition, A = m.inputEnabled,
                        B = u && u.states, C = p.plotLeft, D = d.buttonGroup, E, G = d.options.verticalAlign, I = p.legend, J = I && I.options, K = t.y, N = z.y, O = p.hasLoaded, P = O ? "animate" : "attr", M = 0, L = 0; if (!1 !== m.enabled) {
                            d.rendered || (d.group = E = b.g("range-selector-group").attr({ zIndex: 7 }).add(), d.buttonGroup = D = b.g("range-selector-buttons").add(E), d.zoomText = b.text(l.rangeSelectorZoom, 0, 15).add(D), p.styledMode || (d.zoomText.css(m.labelStyle), u["stroke-width"] = g(u["stroke-width"], 0)), d.buttonOptions.forEach(function (a, c) {
                                v[c] = b.button(a.text, 0, 0, function (b) {
                                    var f =
                                        a.events && a.events.click, e; f && (e = f.call(a, b)); !1 !== e && d.clickButton(c); d.isActive = !0
                                }, u, B && B.hover, B && B.select, B && B.disabled).attr({ "text-align": "center" }).add(D)
                            }), !1 !== A && (d.div = n = c("div", null, { position: "relative", height: 0, zIndex: q }), f.parentNode.insertBefore(n, f), d.inputGroup = n = b.g("input-group").add(E), n.offset = 0, d.drawInput("min"), d.drawInput("max"))); d.zoomText[P]({ x: g(C + t.x, C) }); var Q = g(C + t.x, C) + d.zoomText.getBBox().width + 5; d.buttonOptions.forEach(function (a, b) {
                                v[b][P]({ x: Q }); Q += v[b].width +
                                    g(m.buttonSpacing, 5)
                            }); C = p.plotLeft - p.spacing[3]; d.updateButtonStates(); h && this.titleCollision(p) && "top" === G && "right" === t.align && t.y + D.getBBox().height - 12 < (h.y || 0) + h.height && (M = -40); f = t.x - p.spacing[3]; "right" === t.align ? f += M - C : "center" === t.align && (f -= C / 2); D.align({ y: t.y, width: D.getBBox().width, align: t.align, x: f }, !0, p.spacingBox); d.group.placed = O; d.buttonGroup.placed = O; !1 !== A && (M = h && this.titleCollision(p) && "top" === G && "right" === z.align && z.y - n.getBBox().height - 12 < (h.y || 0) + h.height + p.spacing[0] ? -40 : 0,
                                "left" === z.align ? f = C : "right" === z.align && (f = -Math.max(p.axisOffset[1], -M)), n.align({ y: z.y, width: n.getBBox().width, align: z.align, x: z.x + f - 2 }, !0, p.spacingBox), h = n.alignAttr.translateX + n.alignOptions.x - M + n.getBBox().x + 2, f = n.alignOptions.width, l = D.alignAttr.translateX + D.getBBox().x, C = D.getBBox().width + 20, (z.align === t.align || l + C > h && h + f > l && K < N + n.getBBox().height) && n.attr({ translateX: n.alignAttr.translateX + (p.axisOffset[1] >= -M ? 0 : -M), translateY: n.alignAttr.translateY + D.getBBox().height + 10 }), d.setInputValue("min",
                                    a), d.setInputValue("max", e), d.inputGroup.placed = O); d.group.align({ verticalAlign: G }, !0, p.spacingBox); a = d.group.getBBox().height + 20; e = d.group.alignAttr.translateY; "bottom" === G && (I = J && "bottom" === J.verticalAlign && J.enabled && !J.floating ? I.legendHeight + g(J.margin, 10) : 0, a = a + I - 20, L = e - a - (k ? 0 : m.y) - (p.titleOffset ? p.titleOffset[2] : 0) - 10); if ("top" === G) k && (L = 0), p.titleOffset && p.titleOffset[0] && (L = p.titleOffset[0]), L += p.margin[0] - p.spacing[0] || 0; else if ("middle" === G) if (N === K) L = 0 > N ? e + void 0 : e; else if (N || K) L = 0 > N ||
                                        0 > K ? L - Math.min(N, K) : e - a + NaN; d.group.translate(m.x, m.y + Math.floor(L)); !1 !== A && (d.minInput.style.marginTop = d.group.translateY + "px", d.maxInput.style.marginTop = d.group.translateY + "px"); d.rendered = !0
                        }
                }; e.prototype.getHeight = function () { var a = this.options, c = this.group, e = a.y, g = a.buttonPosition.y, b = a.inputPosition.y; if (a.height) return a.height; a = c ? c.getBBox(!0).height + 13 + e : 0; c = Math.min(b, g); if (0 > b && 0 > g || 0 < b && 0 < g) a += Math.abs(c); return a }; e.prototype.titleCollision = function (a) {
                    return !(a.options.title.text ||
                        a.options.subtitle.text)
                }; e.prototype.update = function (c) { var d = this.chart; a(!0, d.options.rangeSelector, c); this.destroy(); this.init(d); d.rangeSelector.render() }; e.prototype.destroy = function () { var a = this, c = a.minInput, g = a.maxInput; a.unMouseDown(); a.unResize(); G(a.buttons); c && (c.onfocus = c.onblur = c.onchange = null); g && (g.onfocus = g.onblur = g.onchange = null); h(a, function (c, b) { c && "chart" !== b && (c instanceof E ? c.destroy() : c instanceof window.HTMLElement && C(c)); c !== e.prototype[b] && (a[b] = null) }, this) }; return e
            }();
            m.prototype.defaultButtons = [{ type: "month", count: 1, text: "1m" }, { type: "month", count: 3, text: "3m" }, { type: "month", count: 6, text: "6m" }, { type: "ytd", text: "YTD" }, { type: "year", count: 1, text: "1y" }, { type: "all", text: "All" }]; l.prototype.minFromRange = function () {
                var a = this.range, c = a.type, e = this.max, h = this.chart.time, l = function (a, b) { var f = "year" === c ? "FullYear" : "Month", d = new h.Date(a), e = h.get(f, d); h.set(f, d, e + b); e === h.get(f, d) && h.set("Date", d, 0); return d.getTime() - a }; if (n(a)) { var b = e - a; var f = a } else b = e + l(e, -a.count),
                    this.chart && (this.chart.fixedRange = e - b); var m = g(this.dataMin, Number.MIN_VALUE); n(b) || (b = m); b <= m && (b = m, "undefined" === typeof f && (f = l(b, a.count)), this.newMax = Math.min(b + f, this.dataMax)); n(e) || (b = void 0); return b
            }; B.RangeSelector || (q(u, "afterGetContainer", function () { this.options.rangeSelector.enabled && (this.rangeSelector = new m(this)) }), q(u, "beforeRender", function () {
                var a = this.axes, c = this.rangeSelector; c && (n(c.deferredYTDClick) && (c.clickButton(c.deferredYTDClick), delete c.deferredYTDClick), a.forEach(function (a) {
                    a.updateNames();
                    a.setScale()
                }), this.getAxisMargins(), c.render(), a = c.options.verticalAlign, c.options.floating || ("bottom" === a ? this.extraBottomMargin = !0 : "middle" !== a && (this.extraTopMargin = !0)))
            }), q(u, "update", function (a) {
                var c = a.options.rangeSelector; a = this.rangeSelector; var e = this.extraBottomMargin, g = this.extraTopMargin; c && c.enabled && !J(a) && (this.options.rangeSelector.enabled = !0, this.rangeSelector = new m(this)); this.extraTopMargin = this.extraBottomMargin = !1; a && (a.render(), c = c && c.verticalAlign || a.options && a.options.verticalAlign,
                    a.options.floating || ("bottom" === c ? this.extraBottomMargin = !0 : "middle" !== c && (this.extraTopMargin = !0)), this.extraBottomMargin !== e || this.extraTopMargin !== g) && (this.isDirtyBox = !0)
            }), q(u, "render", function () { var a = this.rangeSelector; a && !a.options.floating && (a.render(), a = a.options.verticalAlign, "bottom" === a ? this.extraBottomMargin = !0 : "middle" !== a && (this.extraTopMargin = !0)) }), q(u, "getMargins", function () {
                var a = this.rangeSelector; a && (a = a.getHeight(), this.extraTopMargin && (this.plotTop += a), this.extraBottomMargin &&
                    (this.marginBottom += a))
            }), u.prototype.callbacks.push(function (c) {
                function d() { e = c.xAxis[0].getExtremes(); h = c.legend; f = null === g || void 0 === g ? void 0 : g.options.verticalAlign; n(e.min) && g.render(e.min, e.max); g && h.display && "top" === f && f === h.options.verticalAlign && (b = a(c.spacingBox), b.y = "vertical" === h.options.layout ? c.plotTop : b.y + g.getHeight(), h.group.placed = !1, h.align(b)) } var e, g = c.rangeSelector, h, b, f; if (g) { var k = q(c.xAxis[0], "afterSetExtremes", function (a) { g.render(a.min, a.max) }); var l = q(c, "redraw", d); d() } q(c,
                    "destroy", function () { g && (l(), k()) })
            }), B.RangeSelector = m); return B.RangeSelector
        }); K(l, "parts/StockChart.js", [l["parts/Axis.js"], l["parts/Chart.js"], l["parts/Globals.js"], l["parts/Point.js"], l["parts/SVGRenderer.js"], l["parts/Utilities.js"]], function (l, u, B, t, E, e) {
            var x = e.addEvent, q = e.arrayMax, c = e.arrayMin, v = e.clamp, J = e.defined, G = e.extend, C = e.find, z = e.format, A = e.getOptions, n = e.isNumber, a = e.isString, h = e.merge, g = e.pick, I = e.splat; e = B.Series; var D = e.prototype, m = D.init, k = D.processData, d = t.prototype.tooltipFormatter;
            B.StockChart = B.stockChart = function (c, d, e) {
                var b = a(c) || c.nodeName, f = arguments[b ? 1 : 0], p = f, l = f.series, k = A(), n, m = g(f.navigator && f.navigator.enabled, k.navigator.enabled, !0); f.xAxis = I(f.xAxis || {}).map(function (a, b) { return h({ minPadding: 0, maxPadding: 0, overscroll: 0, ordinal: !0, title: { text: null }, labels: { overflow: "justify" }, showLastLabel: !0 }, k.xAxis, k.xAxis && k.xAxis[b], a, { type: "datetime", categories: null }, m ? { startOnTick: !1, endOnTick: !1 } : null) }); f.yAxis = I(f.yAxis || {}).map(function (a, b) {
                    n = g(a.opposite, !0); return h({
                        labels: { y: -2 },
                        opposite: n, showLastLabel: !(!a.categories && "category" !== a.type), title: { text: null }
                    }, k.yAxis, k.yAxis && k.yAxis[b], a)
                }); f.series = null; f = h({ chart: { panning: { enabled: !0, type: "x" }, pinchType: "x" }, navigator: { enabled: m }, scrollbar: { enabled: g(k.scrollbar.enabled, !0) }, rangeSelector: { enabled: g(k.rangeSelector.enabled, !0) }, title: { text: null }, tooltip: { split: g(k.tooltip.split, !0), crosshairs: !0 }, legend: { enabled: !1 } }, f, { isStock: !0 }); f.series = p.series = l; return b ? new u(c, f, e) : new u(f, d)
            }; x(e, "setOptions", function (a) {
                var c;
                this.chart.options.isStock && (this.is("column") || this.is("columnrange") ? c = { borderWidth: 0, shadow: !1 } : this.is("scatter") || this.is("sma") || (c = { marker: { enabled: !1, radius: 2 } }), c && (a.plotOptions[this.type] = h(a.plotOptions[this.type], c)))
            }); x(l, "autoLabelAlign", function (a) {
                var c = this.chart, d = this.options; c = c._labelPanes = c._labelPanes || {}; var b = this.options.labels; this.chart.options.isStock && "yAxis" === this.coll && (d = d.top + "," + d.height, !c[d] && b.enabled && (15 === b.x && (b.x = 0), "undefined" === typeof b.align && (b.align =
                    "right"), c[d] = this, a.align = "right", a.preventDefault()))
            }); x(l, "destroy", function () { var a = this.chart, c = this.options && this.options.top + "," + this.options.height; c && a._labelPanes && a._labelPanes[c] === this && delete a._labelPanes[c] }); x(l, "getPlotLinePath", function (c) {
                function d(c) { var d = "xAxis" === c ? "yAxis" : "xAxis"; c = e.options[d]; return n(c) ? [f[d][c]] : a(c) ? [f.get(c)] : b.map(function (a) { return a[d] }) } var e = this, b = this.isLinked && !this.series ? this.linkedParent.series : this.series, f = e.chart, h = f.renderer, p = e.left,
                    k = e.top, l, m, q, t, u = [], x = [], z = c.translatedValue, A = c.value, B = c.force; if (f.options.isStock && !1 !== c.acrossPanes && "xAxis" === e.coll || "yAxis" === e.coll) {
                        c.preventDefault(); x = d(e.coll); var D = e.isXAxis ? f.yAxis : f.xAxis; D.forEach(function (a) { if (J(a.options.id) ? -1 === a.options.id.indexOf("navigator") : 1) { var b = a.isXAxis ? "yAxis" : "xAxis"; b = J(a.options[b]) ? f[b][a.options[b]] : f[b][0]; e === b && x.push(a) } }); var E = x.length ? [] : [e.isXAxis ? f.yAxis[0] : f.xAxis[0]]; x.forEach(function (a) {
                            -1 !== E.indexOf(a) || C(E, function (b) {
                                return b.pos ===
                                    a.pos && b.len === a.len
                            }) || E.push(a)
                        }); var G = g(z, e.translate(A, null, null, c.old)); n(G) && (e.horiz ? E.forEach(function (a) { var b; m = a.pos; t = m + a.len; l = q = Math.round(G + e.transB); "pass" !== B && (l < p || l > p + e.width) && (B ? l = q = v(l, p, p + e.width) : b = !0); b || u.push(["M", l, m], ["L", q, t]) }) : E.forEach(function (a) { var b; l = a.pos; q = l + a.len; m = t = Math.round(k + e.height - G); "pass" !== B && (m < k || m > k + e.height) && (B ? m = t = v(m, k, k + e.height) : b = !0); b || u.push(["M", l, m], ["L", q, t]) })); c.path = 0 < u.length ? h.crispPolyLine(u, c.lineWidth || 1) : null
                    }
            }); E.prototype.crispPolyLine =
                function (a, c) { for (var e = 0; e < a.length; e += 2) { var b = a[e], f = a[e + 1]; b[1] === f[1] && (b[1] = f[1] = Math.round(b[1]) - c % 2 / 2); b[2] === f[2] && (b[2] = f[2] = Math.round(b[2]) + c % 2 / 2) } return a }; x(l, "afterHideCrosshair", function () { this.crossLabel && (this.crossLabel = this.crossLabel.hide()) }); x(l, "afterDrawCrosshair", function (a) {
                    var c, e; if (J(this.crosshair.label) && this.crosshair.label.enabled && this.cross) {
                        var b = this.chart, f = this.logarithmic, d = this.options.crosshair.label, h = this.horiz, k = this.opposite, l = this.left, p = this.top, m =
                            this.crossLabel, q = d.format, t = "", u = "inside" === this.options.tickPosition, v = !1 !== this.crosshair.snap, x = 0, A = a.e || this.cross && this.cross.e, B = a.point; a = this.min; var C = this.max; f && (a = f.lin2log(a), C = f.lin2log(C)); f = h ? "center" : k ? "right" === this.labelAlign ? "right" : "left" : "left" === this.labelAlign ? "left" : "center"; m || (m = this.crossLabel = b.renderer.label(null, null, null, d.shape || "callout").addClass("highcharts-crosshair-label" + (this.series[0] && " highcharts-color-" + this.series[0].colorIndex)).attr({
                                align: d.align ||
                                    f, padding: g(d.padding, 8), r: g(d.borderRadius, 3), zIndex: 2
                            }).add(this.labelGroup), b.styledMode || m.attr({ fill: d.backgroundColor || this.series[0] && this.series[0].color || "#666666", stroke: d.borderColor || "", "stroke-width": d.borderWidth || 0 }).css(G({ color: "#ffffff", fontWeight: "normal", fontSize: "11px", textAlign: "center" }, d.style))); h ? (f = v ? B.plotX + l : A.chartX, p += k ? 0 : this.height) : (f = k ? this.width + l : 0, p = v ? B.plotY + p : A.chartY); q || d.formatter || (this.dateTime && (t = "%b %d, %Y"), q = "{value" + (t ? ":" + t : "") + "}"); t = v ? B[this.isXAxis ?
                                "x" : "y"] : this.toValue(h ? A.chartX : A.chartY); m.attr({ text: q ? z(q, { value: t }, b) : d.formatter.call(this, t), x: f, y: p, visibility: t < a || t > C ? "hidden" : "visible" }); d = m.getBBox(); if (n(m.y)) if (h) { if (u && !k || !u && k) p = m.y - d.height } else p = m.y - d.height / 2; h ? (c = l - d.x, e = l + this.width - d.x) : (c = "left" === this.labelAlign ? l : 0, e = "right" === this.labelAlign ? l + this.width : b.chartWidth); m.translateX < c && (x = c - m.translateX); m.translateX + d.width >= e && (x = -(m.translateX + d.width - e)); m.attr({
                                    x: f + x, y: p, anchorX: h ? f : this.opposite ? 0 : b.chartWidth, anchorY: h ?
                                        this.opposite ? b.chartHeight : 0 : p + d.height / 2
                                })
                    }
                }); D.init = function () { m.apply(this, arguments); this.setCompare(this.options.compare) }; D.setCompare = function (a) { this.modifyValue = "value" === a || "percent" === a ? function (c, d) { var b = this.compareValue; return "undefined" !== typeof c && "undefined" !== typeof b ? (c = "value" === a ? c - b : c / b * 100 - (100 === this.options.compareBase ? 0 : 100), d && (d.change = c), c) : 0 } : null; this.userOptions.compare = a; this.chart.hasRendered && (this.isDirty = !0) }; D.processData = function (a) {
                    var c, d = -1, b = !0 === this.options.compareStart ?
                        0 : 1; k.apply(this, arguments); if (this.xAxis && this.processedYData) { var f = this.processedXData; var e = this.processedYData; var g = e.length; this.pointArrayMap && (d = this.pointArrayMap.indexOf(this.options.pointValKey || this.pointValKey || "y")); for (c = 0; c < g - b; c++) { var h = e[c] && -1 < d ? e[c][d] : e[c]; if (n(h) && f[c + b] >= this.xAxis.min && 0 !== h) { this.compareValue = h; break } } }
                }; x(e, "afterGetExtremes", function (a) {
                    a = a.dataExtremes; if (this.modifyValue && a) {
                        var d = [this.modifyValue(a.dataMin), this.modifyValue(a.dataMax)]; a.dataMin = c(d);
                        a.dataMax = q(d)
                    }
                }); l.prototype.setCompare = function (a, c) { this.isXAxis || (this.series.forEach(function (c) { c.setCompare(a) }), g(c, !0) && this.chart.redraw()) }; t.prototype.tooltipFormatter = function (a) { var c = this.series.chart.numberFormatter; a = a.replace("{point.change}", (0 < this.change ? "+" : "") + c(this.change, g(this.series.tooltipOptions.changeDecimals, 2))); return d.apply(this, [a]) }; x(e, "render", function () {
                    var a = this.chart; if (!(a.is3d && a.is3d() || a.polar) && this.xAxis && !this.xAxis.isRadial) {
                        var c = this.yAxis.len;
                        if (this.xAxis.axisLine) { var d = a.plotTop + a.plotHeight - this.yAxis.pos - this.yAxis.len, b = Math.floor(this.xAxis.axisLine.strokeWidth() / 2); 0 <= d && (c -= Math.max(b - d, 0)) } !this.clipBox && this.animate ? (this.clipBox = h(a.clipBox), this.clipBox.width = this.xAxis.len, this.clipBox.height = c) : a[this.sharedClipKey] && (a[this.sharedClipKey].animate({ width: this.xAxis.len, height: c }), a[this.sharedClipKey + "m"] && a[this.sharedClipKey + "m"].animate({ width: this.xAxis.len }))
                    }
                }); x(u, "update", function (a) {
                    a = a.options; "scrollbar" in a &&
                        this.navigator && (h(!0, this.options.scrollbar, a.scrollbar), this.navigator.update({}, !1), delete a.scrollbar)
                })
        }); K(l, "masters/modules/stock.src.js", [], function () { })
});
//# sourceMappingURL=stock.js.map