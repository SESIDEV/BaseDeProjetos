/*
 Highcharts JS v8.1.2 (2020-06-16)

 ColorAxis module

 (c) 2012-2019 Pawel Potaczek

 License: www.highcharts.com/license
*/
(function (b) { "object" === typeof module && module.exports ? (b["default"] = b, module.exports = b) : "function" === typeof define && define.amd ? define("highcharts/modules/color-axis", ["highcharts"], function (m) { b(m); b.Highcharts = m; return b }) : b("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (b) {
    function m(b, k, g, n) { b.hasOwnProperty(k) || (b[k] = n.apply(null, g)) } b = b ? b._modules : {}; m(b, "parts-map/ColorSeriesMixin.js", [b["parts/Globals.js"]], function (b) {
        b.colorPointMixin = {
            setVisible: function (b) {
                var g = this, n = b ?
                    "show" : "hide"; g.visible = g.options.visible = !!b;["graphic", "dataLabel"].forEach(function (b) { if (g[b]) g[b][n]() }); this.series.buildKDTree()
            }
        }; b.colorSeriesMixin = {
            optionalAxis: "colorAxis", colorAxis: 0, translateColors: function () {
                var b = this, g = this.options.nullColor, n = this.colorAxis, m = this.colorKey; (this.data.length ? this.data : this.points).forEach(function (p) {
                    var k = p.getNestedProperty(m); (k = p.options.color || (p.isNull || null === p.value ? g : n && "undefined" !== typeof k ? n.toColor(k, p) : p.color || b.color)) && p.color !== k &&
                        (p.color = k, "point" === b.options.legendType && p.legendItem && b.chart.legend.colorizeItem(p, p.visible))
                })
            }
        }
    }); m(b, "parts-map/ColorAxis.js", [b["parts/Axis.js"], b["parts/Chart.js"], b["parts/Color.js"], b["parts/Globals.js"], b["parts/Legend.js"], b["mixins/legend-symbol.js"], b["parts/Point.js"], b["parts/Utilities.js"]], function (b, k, g, n, m, p, A, q) {
        var B = this && this.__extends || function () {
            var b = function (d, a) {
                b = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, e) { a.__proto__ = e } || function (a, e) {
                    for (var c in e) e.hasOwnProperty(c) &&
                        (a[c] = e[c])
                }; return b(d, a)
            }; return function (d, a) { function f() { this.constructor = d } b(d, a); d.prototype = null === a ? Object.create(a) : (f.prototype = a.prototype, new f) }
        }(), u = g.parse, C = n.noop; g = q.addEvent; var z = q.erase, x = q.extend, D = q.Fx, E = q.isNumber, y = q.merge, t = q.pick, F = q.splat; ""; var v = n.Series; q = n.colorPointMixin; x(v.prototype, n.colorSeriesMixin); x(A.prototype, q); k.prototype.collectionsWithUpdate.push("colorAxis"); k.prototype.collectionsWithInit.colorAxis = [k.prototype.addColorAxis]; var w = function (b) {
            function d(a,
                f) { var e = b.call(this, a, f) || this; e.beforePadding = !1; e.chart = void 0; e.coll = "colorAxis"; e.dataClasses = void 0; e.legendItem = void 0; e.legendItems = void 0; e.name = ""; e.options = void 0; e.stops = void 0; e.visible = !0; e.init(a, f); return e } B(d, b); d.buildOptions = function (a, f, e) { a = a.options.legend || {}; var c = e.layout ? "vertical" !== e.layout : "vertical" !== a.layout; return y(f, { side: c ? 2 : 1, reversed: !c }, e, { opposite: !c, showEmpty: !1, title: null, visible: a.enabled && (e ? !1 !== e.visible : !0) }) }; d.prototype.init = function (a, f) {
                    var e = d.buildOptions(a,
                        d.defaultOptions, f); this.coll = "colorAxis"; b.prototype.init.call(this, a, e); f.dataClasses && this.initDataClasses(f); this.initStops(); this.horiz = !e.opposite; this.zoomEnabled = !1
                }; d.prototype.initDataClasses = function (a) {
                    var f = this.chart, e, c = 0, b = f.options.chart.colorCount, d = this.options, h = a.dataClasses.length; this.dataClasses = e = []; this.legendItems = []; a.dataClasses.forEach(function (a, r) {
                        a = y(a); e.push(a); if (f.styledMode || !a.color) "category" === d.dataClassColor ? (f.styledMode || (r = f.options.colors, b = r.length,
                            a.color = r[c]), a.colorIndex = c, c++, c === b && (c = 0)) : a.color = u(d.minColor).tweenTo(u(d.maxColor), 2 > h ? .5 : r / (h - 1))
                    })
                }; d.prototype.hasData = function () { return !!(this.tickPositions || []).length }; d.prototype.setTickPositions = function () { if (!this.dataClasses) return b.prototype.setTickPositions.call(this) }; d.prototype.initStops = function () { this.stops = this.options.stops || [[0, this.options.minColor], [1, this.options.maxColor]]; this.stops.forEach(function (a) { a.color = u(a[1]) }) }; d.prototype.setOptions = function (a) {
                    b.prototype.setOptions.call(this,
                        a); this.options.crosshair = this.options.marker
                }; d.prototype.setAxisSize = function () { var a = this.legendSymbol, f = this.chart, e = f.options.legend || {}, c, b; a ? (this.left = e = a.attr("x"), this.top = c = a.attr("y"), this.width = b = a.attr("width"), this.height = a = a.attr("height"), this.right = f.chartWidth - e - b, this.bottom = f.chartHeight - c - a, this.len = this.horiz ? b : a, this.pos = this.horiz ? e : c) : this.len = (this.horiz ? e.symbolWidth : e.symbolHeight) || d.defaultLegendLength }; d.prototype.normalizedValue = function (a) {
                    this.logarithmic && (a = this.logarithmic.log2lin(a));
                    return 1 - (this.max - a) / (this.max - this.min || 1)
                }; d.prototype.toColor = function (a, f) { var e = this.dataClasses, c = this.stops, b; if (e) for (b = e.length; b--;) { var d = e[b]; var h = d.from; c = d.to; if (("undefined" === typeof h || a >= h) && ("undefined" === typeof c || a <= c)) { var l = d.color; f && (f.dataClass = b, f.colorIndex = d.colorIndex); break } } else { a = this.normalizedValue(a); for (b = c.length; b-- && !(a > c[b][0]);); h = c[b] || c[b + 1]; c = c[b + 1] || h; a = 1 - (c[0] - a) / (c[0] - h[0] || 1); l = h.color.tweenTo(c.color, a) } return l }; d.prototype.getOffset = function () {
                    var a =
                        this.legendGroup, f = this.chart.axisOffset[this.side]; a && (this.axisParent = a, b.prototype.getOffset.call(this), this.added || (this.added = !0, this.labelLeft = 0, this.labelRight = this.width), this.chart.axisOffset[this.side] = f)
                }; d.prototype.setLegendColor = function () { var a = this.reversed, f = a ? 1 : 0; a = a ? 0 : 1; f = this.horiz ? [f, 0, a, 0] : [0, a, 0, f]; this.legendColor = { linearGradient: { x1: f[0], y1: f[1], x2: f[2], y2: f[3] }, stops: this.stops } }; d.prototype.drawLegendSymbol = function (a, f) {
                    var b = a.padding, c = a.options, r = this.horiz, l = t(c.symbolWidth,
                        r ? d.defaultLegendLength : 12), h = t(c.symbolHeight, r ? 12 : d.defaultLegendLength), g = t(c.labelPadding, r ? 16 : 30); c = t(c.itemDistance, 10); this.setLegendColor(); f.legendSymbol = this.chart.renderer.rect(0, a.baseline - 11, l, h).attr({ zIndex: 1 }).add(f.legendGroup); this.legendItemWidth = l + b + (r ? c : g); this.legendItemHeight = h + b + (r ? g : 0)
                }; d.prototype.setState = function (a) { this.series.forEach(function (b) { b.setState(a) }) }; d.prototype.setVisible = function () { }; d.prototype.getSeriesExtremes = function () {
                    var a = this.series, b = a.length,
                    e; this.dataMin = Infinity; for (this.dataMax = -Infinity; b--;) {
                        var c = a[b]; var d = c.colorKey = t(c.options.colorKey, c.colorKey, c.pointValKey, c.zoneAxis, "y"); var l = c.pointArrayMap; var h = c[d + "Min"] && c[d + "Max"]; if (c[d + "Data"]) var g = c[d + "Data"]; else if (l) { g = []; l = l.indexOf(d); var k = c.yData; if (0 <= l && k) for (e = 0; e < k.length; e++)g.push(t(k[e][l], k[e])) } else g = c.yData; h ? (c.minColorValue = c[d + "Min"], c.maxColorValue = c[d + "Max"]) : (g = v.prototype.getExtremes.call(c, g), c.minColorValue = g.dataMin, c.maxColorValue = g.dataMax); "undefined" !==
                            typeof c.minColorValue && (this.dataMin = Math.min(this.dataMin, c.minColorValue), this.dataMax = Math.max(this.dataMax, c.maxColorValue)); h || v.prototype.applyExtremes.call(c)
                    }
                }; d.prototype.drawCrosshair = function (a, f) {
                    var d = f && f.plotX, c = f && f.plotY, l = this.pos, g = this.len; if (f) {
                        var h = this.toPixels(f.getNestedProperty(f.series.colorKey)); h < l ? h = l - 2 : h > l + g && (h = l + g + 2); f.plotX = h; f.plotY = this.len - h; b.prototype.drawCrosshair.call(this, a, f); f.plotX = d; f.plotY = c; this.cross && !this.cross.addedToColorAxis && this.legendGroup &&
                            (this.cross.addClass("highcharts-coloraxis-marker").add(this.legendGroup), this.cross.addedToColorAxis = !0, !this.chart.styledMode && this.crosshair && this.cross.attr({ fill: this.crosshair.color }))
                    }
                }; d.prototype.getPlotLinePath = function (a) { var d = this.left, e = a.translatedValue, c = this.top; return E(e) ? this.horiz ? [["M", e - 4, c - 6], ["L", e + 4, c - 6], ["L", e, c], ["Z"]] : [["M", d, e], ["L", d - 6, e + 6], ["L", d - 6, e - 6], ["Z"]] : b.prototype.getPlotLinePath.call(this, a) }; d.prototype.update = function (a, f) {
                    var e = this.chart, c = e.legend, l = d.buildOptions(e,
                        {}, a); this.series.forEach(function (a) { a.isDirtyData = !0 }); (a.dataClasses && c.allItems || this.dataClasses) && this.destroyItems(); e.options[this.coll] = y(this.userOptions, l); b.prototype.update.call(this, l, f); this.legendItem && (this.setLegendColor(), c.colorizeItem(this, !0))
                }; d.prototype.destroyItems = function () { var a = this.chart; this.legendItem ? a.legend.destroyItem(this) : this.legendItems && this.legendItems.forEach(function (b) { a.legend.destroyItem(b) }); a.isDirtyLegend = !0 }; d.prototype.remove = function (a) {
                    this.destroyItems();
                    b.prototype.remove.call(this, a)
                }; d.prototype.getDataClassLegendSymbols = function () {
                    var a = this, b = a.chart, d = a.legendItems, c = b.options.legend, l = c.valueDecimals, g = c.valueSuffix || "", h; d.length || a.dataClasses.forEach(function (c, e) {
                        var f = !0, k = c.from, m = c.to, n = b.numberFormatter; h = ""; "undefined" === typeof k ? h = "< " : "undefined" === typeof m && (h = "> "); "undefined" !== typeof k && (h += n(k, l) + g); "undefined" !== typeof k && "undefined" !== typeof m && (h += " - "); "undefined" !== typeof m && (h += n(m, l) + g); d.push(x({
                            chart: b, name: h, options: {},
                            drawLegendSymbol: p.drawRectangle, visible: !0, setState: C, isDataClass: !0, setVisible: function () { f = a.visible = !f; a.series.forEach(function (a) { a.points.forEach(function (a) { a.dataClass === e && a.setVisible(f) }) }); b.legend.colorizeItem(this, f) }
                        }, c))
                    }); return d
                }; d.defaultLegendLength = 200; d.defaultOptions = {
                    lineWidth: 0, minPadding: 0, maxPadding: 0, gridLineWidth: 1, tickPixelInterval: 72, startOnTick: !0, endOnTick: !0, offset: 0, marker: { animation: { duration: 50 }, width: .01, color: "#999999" }, labels: { overflow: "justify", rotation: 0 },
                    minColor: "#e6ebf5", maxColor: "#003399", tickLength: 5, showInLegend: !0
                }; d.keepProps = ["legendGroup", "legendItemHeight", "legendItemWidth", "legendItem", "legendSymbol"]; return d
        }(b); Array.prototype.push.apply(b.keepProps, w.keepProps); n.ColorAxis = w;["fill", "stroke"].forEach(function (b) { D.prototype[b + "Setter"] = function () { this.elem.attr(b, u(this.start).tweenTo(u(this.end), this.pos), null, !0) } }); g(k, "afterGetAxes", function () {
            var b = this, d = b.options; this.colorAxis = []; d.colorAxis && (d.colorAxis = F(d.colorAxis), d.colorAxis.forEach(function (a,
                d) { a.index = d; new w(b, a) }))
        }); g(v, "bindAxes", function () { var b = this.axisTypes; b ? -1 === b.indexOf("colorAxis") && b.push("colorAxis") : this.axisTypes = ["colorAxis"] }); g(m, "afterGetAllItems", function (b) {
            var d = [], a, f; (this.chart.colorAxis || []).forEach(function (e) {
                (a = e.options) && a.showInLegend && (a.dataClasses && a.visible ? d = d.concat(e.getDataClassLegendSymbols()) : a.visible && d.push(e), e.series.forEach(function (c) {
                    if (!c.options.showInLegend || a.dataClasses) "point" === c.options.legendType ? c.points.forEach(function (a) {
                        z(b.allItems,
                            a)
                    }) : z(b.allItems, c)
                }))
            }); for (f = d.length; f--;)b.allItems.unshift(d[f])
        }); g(m, "afterColorizeItem", function (b) { b.visible && b.item.legendColor && b.item.legendSymbol.attr({ fill: b.item.legendColor }) }); g(m, "afterUpdate", function () { var b = this.chart.colorAxis; b && b.forEach(function (b, a, f) { b.update({}, f) }) }); g(v, "afterTranslate", function () { (this.chart.colorAxis && this.chart.colorAxis.length || this.colorAttribs) && this.translateColors() }); return w
    }); m(b, "masters/modules/coloraxis.src.js", [], function () { })
});
//# sourceMappingURL=coloraxis.js.map