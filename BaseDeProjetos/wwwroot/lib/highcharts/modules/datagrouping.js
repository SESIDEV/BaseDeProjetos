/*
 Highstock JS v8.1.2 (2020-06-16)

 Data grouping module

 (c) 2010-2019 Torstein Hnsi

 License: www.highcharts.com/license
*/
(function (d) { "object" === typeof module && module.exports ? (d["default"] = d, module.exports = d) : "function" === typeof define && define.amd ? define("highcharts/modules/datagrouping", ["highcharts"], function (g) { d(g); d.Highcharts = g; return d }) : d("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (d) {
    function g(d, m, g, D) { d.hasOwnProperty(m) || (d[m] = D.apply(null, g)) } d = d ? d._modules : {}; g(d, "parts/DataGrouping.js", [d["parts/DateTimeAxis.js"], d["parts/Globals.js"], d["parts/Options.js"], d["parts/Point.js"], d["parts/Tooltip.js"],
    d["parts/Utilities.js"]], function (d, m, g, D, K, f) {
        ""; var y = f.addEvent, C = f.arrayMax, L = f.arrayMin, M = f.correctFloat, E = f.defined, N = f.error, O = f.extend, P = f.format, x = f.isNumber, F = f.merge, G = f.pick, B = m.Axis; f = m.Series; var u = m.approximations = {
            sum: function (a) { var c = a.length; if (!c && a.hasNulls) var b = null; else if (c) for (b = 0; c--;)b += a[c]; return b }, average: function (a) { var c = a.length; a = u.sum(a); x(a) && c && (a = M(a / c)); return a }, averages: function () {
                var a = [];[].forEach.call(arguments, function (c) { a.push(u.average(c)) }); return "undefined" ===
                    typeof a[0] ? void 0 : a
            }, open: function (a) { return a.length ? a[0] : a.hasNulls ? null : void 0 }, high: function (a) { return a.length ? C(a) : a.hasNulls ? null : void 0 }, low: function (a) { return a.length ? L(a) : a.hasNulls ? null : void 0 }, close: function (a) { return a.length ? a[a.length - 1] : a.hasNulls ? null : void 0 }, ohlc: function (a, c, b, h) { a = u.open(a); c = u.high(c); b = u.low(b); h = u.close(h); if (x(a) || x(c) || x(b) || x(h)) return [a, c, b, h] }, range: function (a, c) { a = u.low(a); c = u.high(c); if (x(a) || x(c)) return [a, c]; if (null === a && null === c) return null }
        }, H = function (a,
            c, b, h) {
                var e = this, d = e.data, v = e.options && e.options.data, A = [], n = [], f = [], p = a.length, q = !!c, r = [], k = e.pointArrayMap, g = k && k.length, w = ["x"].concat(k || ["y"]), m = 0, y = 0, t; h = "function" === typeof h ? h : u[h] ? u[h] : u[e.getDGApproximation && e.getDGApproximation() || "average"]; g ? k.forEach(function () { r.push([]) }) : r.push([]); var B = g || 1; for (t = 0; t <= p && !(a[t] >= b[0]); t++); for (t; t <= p; t++) {
                    for (; "undefined" !== typeof b[m + 1] && a[t] >= b[m + 1] || t === p;) {
                        var l = b[m]; e.dataGroupInfo = { start: e.cropStart + y, length: r[0].length }; var z = h.apply(e,
                            r); e.pointClass && !E(e.dataGroupInfo.options) && (e.dataGroupInfo.options = F(e.pointClass.prototype.optionsToObject.call({ series: e }, e.options.data[e.cropStart + y])), w.forEach(function (a) { delete e.dataGroupInfo.options[a] })); "undefined" !== typeof z && (A.push(l), n.push(z), f.push(e.dataGroupInfo)); y = t; for (l = 0; l < B; l++)r[l].length = 0, r[l].hasNulls = !1; m += 1; if (t === p) break
                    } if (t === p) break; if (k) for (l = e.cropStart + t, z = d && d[l] || e.pointClass.prototype.applyOptions.apply({ series: e }, [v[l]]), l = 0; l < g; l++) {
                        var C = z[k[l]]; x(C) ?
                            r[l].push(C) : null === C && (r[l].hasNulls = !0)
                    } else l = q ? c[t] : null, x(l) ? r[0].push(l) : null === l && (r[0].hasNulls = !0)
                } return { groupedXData: A, groupedYData: n, groupMap: f }
        }, I = { approximations: u, groupData: H }, w = f.prototype, Q = w.processData, R = w.generatePoints, z = {
            groupPixelWidth: 2, dateTimeLabelFormats: {
                millisecond: ["%A, %b %e, %H:%M:%S.%L", "%A, %b %e, %H:%M:%S.%L", "-%H:%M:%S.%L"], second: ["%A, %b %e, %H:%M:%S", "%A, %b %e, %H:%M:%S", "-%H:%M:%S"], minute: ["%A, %b %e, %H:%M", "%A, %b %e, %H:%M", "-%H:%M"], hour: ["%A, %b %e, %H:%M",
                    "%A, %b %e, %H:%M", "-%H:%M"], day: ["%A, %b %e, %Y", "%A, %b %e", "-%A, %b %e, %Y"], week: ["Week from %A, %b %e, %Y", "%A, %b %e", "-%A, %b %e, %Y"], month: ["%B %Y", "%B", "-%B %Y"], year: ["%Y", "%Y", "-%Y"]
            }
        }, J = { line: {}, spline: {}, area: {}, areaspline: {}, arearange: {}, column: { groupPixelWidth: 10 }, columnrange: { groupPixelWidth: 10 }, candlestick: { groupPixelWidth: 10 }, ohlc: { groupPixelWidth: 5 } }, S = m.defaultDataGroupingUnits = [["millisecond", [1, 2, 5, 10, 20, 25, 50, 100, 200, 500]], ["second", [1, 2, 5, 10, 15, 30]], ["minute", [1, 2, 5, 10, 15,
            30]], ["hour", [1, 2, 3, 4, 6, 8, 12]], ["day", [1]], ["week", [1]], ["month", [1, 3, 6]], ["year", null]]; w.getDGApproximation = function () { return this.is("arearange") ? "range" : this.is("ohlc") ? "ohlc" : this.is("column") ? "sum" : "average" }; w.groupData = H; w.processData = function () {
                var a = this.chart, c = this.options.dataGrouping, b = !1 !== this.allowDG && c && G(c.enabled, a.options.isStock), h = this.visible || !a.options.chart.ignoreHiddenSeries, e, f = this.currentDataGrouping, v = !1; this.forceCrop = b; this.groupPixelWidth = null; this.hasProcessed =
                    !0; b && !this.requireSorting && (this.requireSorting = v = !0); b = !1 === Q.apply(this, arguments) || !b; v && (this.requireSorting = !1); if (!b) {
                        this.destroyGroupedData(); b = c.groupAll ? this.xData : this.processedXData; var A = c.groupAll ? this.yData : this.processedYData, n = a.plotSizeX; a = this.xAxis; var g = a.options.ordinal, p = this.groupPixelWidth = a.getGroupPixelWidth && a.getGroupPixelWidth(); if (p) {
                            this.isDirty = e = !0; this.points = null; v = a.getExtremes(); var q = v.min; v = v.max; g = g && a.ordinal && a.ordinal.getGroupIntervalFactor(q, v, this) ||
                                1; p = p * (v - q) / n * g; n = a.getTimeTicks(d.AdditionsClass.prototype.normalizeTimeTickInterval(p, c.units || S), Math.min(q, b[0]), Math.max(v, b[b.length - 1]), a.options.startOfWeek, b, this.closestPointRange); A = w.groupData.apply(this, [b, A, n, c.approximation]); b = A.groupedXData; g = A.groupedYData; var r = 0; if (c.smoothed && b.length) { var k = b.length - 1; for (b[k] = Math.min(b[k], v); k-- && 0 < k;)b[k] += p / 2; b[0] = Math.max(b[0], q) } for (k = 1; k < n.length; k++)n.info.segmentStarts && -1 !== n.info.segmentStarts.indexOf(k) || (r = Math.max(n[k] - n[k - 1], r));
                            q = n.info; q.gapSize = r; this.closestPointRange = n.info.totalRange; this.groupMap = A.groupMap; if (E(b[0]) && b[0] < a.min && h) { if (!E(a.options.min) && a.min <= a.dataMin || a.min === a.dataMin) a.min = Math.min(b[0], a.min); a.dataMin = Math.min(b[0], a.dataMin) } c.groupAll && (c = this.cropData(b, g, a.min, a.max, 1), b = c.xData, g = c.yData); this.processedXData = b; this.processedYData = g
                        } else this.groupMap = null; this.hasGroupedData = e; this.currentDataGrouping = q; this.preventGraphAnimation = (f && f.totalRange) !== (q && q.totalRange)
                    }
            }; w.destroyGroupedData =
                function () { this.groupedData && (this.groupedData.forEach(function (a, c) { a && (this.groupedData[c] = a.destroy ? a.destroy() : null) }, this), this.groupedData.length = 0) }; w.generatePoints = function () { R.apply(this); this.destroyGroupedData(); this.groupedData = this.hasGroupedData ? this.points : null }; y(D, "update", function () { if (this.dataGroup) return N(24, !1, this.series.chart), !1 }); y(K, "headerFormatter", function (a) {
                    var c = this.chart, b = c.time, h = a.labelConfig, e = h.series, d = e.tooltipOptions, g = e.options.dataGrouping, f = d.xDateFormat,
                    n = e.xAxis, m = d[(a.isFooter ? "footer" : "header") + "Format"]; if (n && "datetime" === n.options.type && g && x(h.key)) { var p = e.currentDataGrouping; g = g.dateTimeLabelFormats || z.dateTimeLabelFormats; if (p) if (d = g[p.unitName], 1 === p.count) f = d[0]; else { f = d[1]; var q = d[2] } else !f && g && (f = this.getXDateFormat(h, d, n)); f = b.dateFormat(f, h.key); q && (f += b.dateFormat(q, h.key + p.totalRange - 1)); e.chart.styledMode && (m = this.styledModeFormat(m)); a.text = P(m, { point: O(h.point, { key: f }), series: e }, c); a.preventDefault() }
                }); y(f, "destroy", w.destroyGroupedData);
        y(f, "afterSetOptions", function (a) { a = a.options; var c = this.type, b = this.chart.options.plotOptions, d = g.defaultOptions.plotOptions[c].dataGrouping, e = this.useCommonDataGrouping && z; if (J[c] || e) d || (d = F(z, J[c])), a.dataGrouping = F(e, d, b.series && b.series.dataGrouping, b[c].dataGrouping, this.userOptions.dataGrouping) }); y(B, "afterSetScale", function () { this.series.forEach(function (a) { a.hasProcessed = !1 }) }); B.prototype.getGroupPixelWidth = function () {
            var a = this.series, c = a.length, b, d = 0, e = !1, f; for (b = c; b--;)(f = a[b].options.dataGrouping) &&
                (d = Math.max(d, G(f.groupPixelWidth, z.groupPixelWidth))); for (b = c; b--;)(f = a[b].options.dataGrouping) && a[b].hasProcessed && (c = (a[b].processedXData || a[b].data).length, a[b].groupPixelWidth || c > this.chart.plotSizeX / d || c && f.forced) && (e = !0); return e ? d : 0
        }; B.prototype.setDataGrouping = function (a, c) {
            var b; c = G(c, !0); a || (a = { forced: !1, units: null }); if (this instanceof B) for (b = this.series.length; b--;)this.series[b].update({ dataGrouping: a }, !1); else this.chart.options.series.forEach(function (b) { b.dataGrouping = a }, !1); this.ordinal &&
                (this.ordinal.slope = void 0); c && this.chart.redraw()
        }; m.dataGrouping = I; ""; return I
    }); g(d, "masters/modules/datagrouping.src.js", [d["parts/DataGrouping.js"]], function (d) { return d })
});
//# sourceMappingURL=datagrouping.js.map