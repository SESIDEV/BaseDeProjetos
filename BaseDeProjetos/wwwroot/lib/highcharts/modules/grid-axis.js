/*
 Highcharts Gantt JS v8.1.2 (2020-06-16)

 GridAxis

 (c) 2016-2019 Lars A. V. Cabrera

 License: www.highcharts.com/license
*/
(function (d) { "object" === typeof module && module.exports ? (d["default"] = d, module.exports = d) : "function" === typeof define && define.amd ? define("highcharts/modules/grid-axis", ["highcharts"], function (n) { d(n); d.Highcharts = n; return d }) : d("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (d) {
    function n(d, x, n, B) { d.hasOwnProperty(x) || (d[x] = B.apply(null, n)) } d = d ? d._modules : {}; n(d, "parts-gantt/GridAxis.js", [d["parts/Axis.js"], d["parts/Globals.js"], d["parts/Options.js"], d["parts/Tick.js"], d["parts/Utilities.js"]],
        function (d, n, A, B, l) {
            var x = A.dateFormat, f = l.addEvent, p = l.defined, C = l.erase, E = l.find, F = l.isArray, v = l.isNumber, y = l.merge, z = l.pick, G = l.timeUnits, D = l.wrap; A = n.Chart; var H = function (b) { var a = b.options; a.labels || (a.labels = {}); a.labels.align = z(a.labels.align, "center"); b.categories || (a.showLastLabel = !1); b.labelRotation = 0; a.labels.rotation = 0 }; ""; d.prototype.getMaxLabelDimensions = function (b, a) {
                var c = { width: 0, height: 0 }; a.forEach(function (a) {
                    a = b[a]; if (l.isObject(a, !0)) {
                        var e = l.isObject(a.label, !0) ? a.label : {}; a =
                            e.getBBox ? e.getBBox().height : 0; e.textStr && !v(e.textPxLength) && (e.textPxLength = e.getBBox().width); e = v(e.textPxLength) ? Math.round(e.textPxLength) : 0; c.height = Math.max(a, c.height); c.width = Math.max(e, c.width)
                    }
                }); return c
            }; n.dateFormats.W = function (b) {
                b = new this.Date(b); var a = (this.get("Day", b) + 6) % 7, c = new this.Date(b.valueOf()); this.set("Date", c, this.get("Date", b) - a + 3); a = new this.Date(this.get("FullYear", c), 0, 1); 4 !== this.get("Day", a) && (this.set("Month", b, 0), this.set("Date", b, 1 + (11 - this.get("Day", a)) % 7));
                return (1 + Math.floor((c.valueOf() - a.valueOf()) / 6048E5)).toString()
            }; n.dateFormats.E = function (b) { return x("%a", b, !0).charAt(0) }; f(A, "afterSetChartSize", function () { this.axes.forEach(function (b) { (b.grid && b.grid.columns || []).forEach(function (a) { a.setAxisSize(); a.setAxisTranslation() }) }) }); f(B, "afterGetLabelPosition", function (b) {
                var a = this.label, c = this.axis, m = c.reversed, e = c.chart, k = c.options.grid || {}, g = c.options.labels, u = g.align, h = w.Side[c.side], d = b.tickmarkOffset, r = c.tickPositions, q = this.pos - d; r = v(r[b.index +
                    1]) ? r[b.index + 1] - d : c.max + d; var t = c.tickSize("tick"); d = t ? t[0] : 0; t = t ? t[1] / 2 : 0; if (!0 === k.enabled) {
                        if ("top" === h) { k = c.top + c.offset; var f = k - d } else "bottom" === h ? (f = e.chartHeight - c.bottom + c.offset, k = f + d) : (k = c.top + c.len - c.translate(m ? r : q), f = c.top + c.len - c.translate(m ? q : r)); "right" === h ? (h = e.chartWidth - c.right + c.offset, m = h + d) : "left" === h ? (m = c.left + c.offset, h = m - d) : (h = Math.round(c.left + c.translate(m ? r : q)) - t, m = Math.round(c.left + c.translate(m ? q : r)) - t); this.slotWidth = m - h; b.pos.x = "left" === u ? h : "right" === u ? m : h + (m - h) / 2;
                        b.pos.y = f + (k - f) / 2; e = e.renderer.fontMetrics(g.style.fontSize, a.element); a = a.getBBox().height; g.useHTML ? b.pos.y += e.b + -(a / 2) : (a = Math.round(a / e.h), b.pos.y += (e.b - (e.h - e.f)) / 2 + -((a - 1) * e.h / 2)); b.pos.x += c.horiz && g.x || 0
                    }
            }); var I = function () {
                function b(a) { this.axis = a } b.prototype.isOuterAxis = function () {
                    var a = this.axis, c = a.grid.columnIndex, b = a.linkedParent && a.linkedParent.grid.columns || a.grid.columns, e = c ? a.linkedParent : a, d = -1, g = 0; a.chart[a.coll].forEach(function (c, b) {
                        c.side !== a.side || c.options.isInternal || (g =
                            b, c === e && (d = b))
                    }); return g === d && (v(c) ? b.length === c : !0)
                }; return b
            }(), w = function () {
                function b() { } b.compose = function (a) {
                    d.keepProps.push("grid"); D(a.prototype, "unsquish", b.wrapUnsquish); f(a, "init", b.onInit); f(a, "afterGetOffset", b.onAfterGetOffset); f(a, "afterGetTitlePosition", b.onAfterGetTitlePosition); f(a, "afterInit", b.onAfterInit); f(a, "afterRender", b.onAfterRender); f(a, "afterSetAxisTranslation", b.onAfterSetAxisTranslation); f(a, "afterSetOptions", b.onAfterSetOptions); f(a, "afterSetOptions", b.onAfterSetOptions2);
                    f(a, "afterSetScale", b.onAfterSetScale); f(a, "afterTickSize", b.onAfterTickSize); f(a, "trimTicks", b.onTrimTicks); f(a, "destroy", b.onDestroy)
                }; b.onAfterGetOffset = function () { var a = this.grid; (a && a.columns || []).forEach(function (a) { a.getOffset() }) }; b.onAfterGetTitlePosition = function (a) {
                    if (!0 === (this.options.grid || {}).enabled) {
                        var c = this.axisTitle, d = this.height, e = this.horiz, k = this.left, g = this.offset, u = this.opposite, h = this.options.title, f = void 0 === h ? {} : h; h = this.top; var r = this.width, q = this.tickSize(), t = c && c.getBBox().width,
                            l = f.x || 0, n = f.y || 0, p = z(f.margin, e ? 5 : 10); c = this.chart.renderer.fontMetrics(f.style && f.style.fontSize, c).f; q = (e ? h + d : k) + (e ? 1 : -1) * (u ? -1 : 1) * (q ? q[0] / 2 : 0) + (this.side === b.Side.bottom ? c : 0); a.titlePosition.x = e ? k - t / 2 - p + l : q + (u ? r : 0) + g + l; a.titlePosition.y = e ? q - (u ? d : 0) + (u ? c : -c) / 2 + g + n : h - p + n
                    }
                }; b.onAfterInit = function () {
                    var a = this.chart, c = this.options.grid; c = void 0 === c ? {} : c; var b = this.userOptions; c.enabled && (H(this), D(this, "labelFormatter", function (a) {
                        var c = this.axis, b = this.value, e = c.tickPositions, d = (c.isLinked ? c.linkedParent :
                            c).series[0], m = b === e[0]; e = b === e[e.length - 1]; d = d && E(d.options.data, function (a) { return a[c.isXAxis ? "x" : "y"] === b }); this.isFirst = m; this.isLast = e; this.point = d; return a.call(this)
                    })); if (c.columns) for (var e = this.grid.columns = [], k = this.grid.columnIndex = 0; ++k < c.columns.length;) { var g = y(b, c.columns[c.columns.length - k - 1], { linkedTo: 0, type: "category" }); delete g.grid.columns; g = new d(this.chart, g); g.grid.isColumn = !0; g.grid.columnIndex = k; C(a.axes, g); C(a[this.coll], g); e.push(g) }
                }; b.onAfterRender = function () {
                    var a =
                        this.grid, c = this.options, d = this.chart.renderer; if (!0 === (c.grid || {}).enabled) {
                            this.maxLabelDimensions = this.getMaxLabelDimensions(this.ticks, this.tickPositions); this.rightWall && this.rightWall.destroy(); if (this.grid && this.grid.isOuterAxis() && this.axisLine) {
                                var e = c.lineWidth; if (e) {
                                    var k = this.getLinePath(e), g = k[0], f = k[1], h = ((this.tickSize("tick") || [1])[0] - 1) * (this.side === b.Side.top || this.side === b.Side.left ? -1 : 1); "M" === g[0] && "L" === f[0] && (this.horiz ? (g[2] += h, f[2] += h) : (g[1] += h, f[1] += h)); this.grid.axisLineExtra ?
                                        this.grid.axisLineExtra.animate({ d: k }) : (this.grid.axisLineExtra = d.path(k).attr({ zIndex: 7 }).addClass("highcharts-axis-line").add(this.axisGroup), d.styledMode || this.grid.axisLineExtra.attr({ stroke: c.lineColor, "stroke-width": e })); this.axisLine[this.showAxis ? "show" : "hide"](!0)
                                }
                            } (a && a.columns || []).forEach(function (a) { a.render() })
                        }
                }; b.onAfterSetAxisTranslation = function () {
                    var a = this.tickPositions && this.tickPositions.info, c = this.options, b = c.grid || {}, e = this.userOptions.labels || {}; this.horiz && (!0 === b.enabled &&
                        this.series.forEach(function (a) { a.options.pointRange = 0 }), a && c.dateTimeLabelFormats && c.labels && !p(e.align) && (!1 === c.dateTimeLabelFormats[a.unitName].range || 1 < a.count) && (c.labels.align = "left", p(e.x) || (c.labels.x = 3)))
                }; b.onAfterSetOptions = function (a) {
                    var c = this.options; a = a.userOptions; var b = c && l.isObject(c.grid, !0) ? c.grid : {}; if (!0 === b.enabled) {
                        var e = y(!0, {
                            className: "highcharts-grid-axis " + (a.className || ""), dateTimeLabelFormats: {
                                hour: { list: ["%H:%M", "%H"] }, day: { list: ["%A, %e. %B", "%a, %e. %b", "%E"] }, week: {
                                    list: ["Week %W",
                                        "W%W"]
                                }, month: { list: ["%B", "%b", "%o"] }
                            }, grid: { borderWidth: 1 }, labels: { padding: 2, style: { fontSize: "13px" } }, margin: 0, title: { text: null, reserveSpace: !1, rotation: 0 }, units: [["millisecond", [1, 10, 100]], ["second", [1, 10]], ["minute", [1, 5, 15]], ["hour", [1, 6]], ["day", [1]], ["week", [1]], ["month", [1]], ["year", null]]
                        }, a); "xAxis" === this.coll && (p(a.linkedTo) && !p(a.tickPixelInterval) && (e.tickPixelInterval = 350), p(a.tickPixelInterval) || !p(a.linkedTo) || p(a.tickPositioner) || p(a.tickInterval) || (e.tickPositioner = function (a, c) {
                            var b =
                                this.linkedParent && this.linkedParent.tickPositions && this.linkedParent.tickPositions.info; if (b) { var d, f = e.units; for (d = 0; d < f.length; d++)if (f[d][0] === b.unitName) { var g = d; break } if (f[g + 1]) { var m = f[g + 1][0]; var k = (f[g + 1][1] || [1])[0] } else "year" === b.unitName && (m = "year", k = 10 * b.count); b = G[m]; this.tickInterval = b * k; return this.getTimeTicks({ unitRange: b, count: k, unitName: m }, a, c, this.options.startOfWeek) }
                        })); y(!0, this.options, e); this.horiz && (c.minPadding = z(a.minPadding, 0), c.maxPadding = z(a.maxPadding, 0)); v(c.grid.borderWidth) &&
                            (c.tickWidth = c.lineWidth = b.borderWidth)
                    }
                }; b.onAfterSetOptions2 = function (a) { a = (a = a.userOptions) && a.grid || {}; var c = a.columns; a.enabled && c && y(!0, this.options, c[c.length - 1]) }; b.onAfterSetScale = function () { (this.grid.columns || []).forEach(function (a) { a.setScale() }) }; b.onAfterTickSize = function (a) {
                    var c = d.defaultLeftAxisOptions, b = this.horiz, e = this.maxLabelDimensions, f = this.options.grid; f = void 0 === f ? {} : f; f.enabled && e && (c = 2 * Math.abs(c.labels.x), b = b ? f.cellHeight || c + e.height : c + e.width, F(a.tickSize) ? a.tickSize[0] =
                        b : a.tickSize = [b, 0])
                }; b.onDestroy = function (a) { var c = this.grid; (c.columns || []).forEach(function (c) { c.destroy(a.keepEvents) }); c.columns = void 0 }; b.onInit = function (a) { a = a.userOptions || {}; var c = a.grid || {}; c.enabled && p(c.borderColor) && (a.tickColor = a.lineColor = c.borderColor); this.grid || (this.grid = new I(this)) }; b.onTrimTicks = function () {
                    var a = this.options, c = this.categories, b = this.tickPositions, d = b[0], f = b[b.length - 1], g = this.linkedParent && this.linkedParent.min || this.min, l = this.linkedParent && this.linkedParent.max ||
                        this.max, h = this.tickInterval; !0 !== (a.grid || {}).enabled || c || !this.horiz && !this.isLinked || (d < g && d + h > g && !a.startOnTick && (b[0] = g), f > l && f - h < l && !a.endOnTick && (b[b.length - 1] = l))
                }; b.wrapUnsquish = function (a) { var b = this.options.grid; return !0 === (void 0 === b ? {} : b).enabled && this.categories ? this.tickInterval : a.apply(this, Array.prototype.slice.call(arguments, 1)) }; return b
            }(); (function (b) { b = b.Side || (b.Side = {}); b[b.top = 0] = "top"; b[b.right = 1] = "right"; b[b.bottom = 2] = "bottom"; b[b.left = 3] = "left" })(w || (w = {})); w.compose(d);
            return w
        }); n(d, "masters/modules/grid-axis.src.js", [], function () { })
});
//# sourceMappingURL=grid-axis.js.map