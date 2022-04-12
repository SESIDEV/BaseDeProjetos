/*
 Highcharts Gantt JS v8.1.2 (2020-06-16)

 Pathfinder

 (c) 2016-2019 ystein Moseng

 License: www.highcharts.com/license
*/
(function (e) { "object" === typeof module && module.exports ? (e["default"] = e, module.exports = e) : "function" === typeof define && define.amd ? define("highcharts/modules/pathfinder", ["highcharts"], function (C) { e(C); e.Highcharts = C; return e }) : e("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (e) {
    function C(e, l, r, t) { e.hasOwnProperty(l) || (e[l] = t.apply(null, r)) } e = e ? e._modules : {}; C(e, "parts-gantt/PathfinderAlgorithms.js", [e["parts/Utilities.js"]], function (e) {
        function l(c, b, g) {
            g = g || 0; var e = c.length - 1; b -= 1e-7;
            for (var l, p; g <= e;)if (l = e + g >> 1, p = b - c[l].xMin, 0 < p) g = l + 1; else if (0 > p) e = l - 1; else return l; return 0 < g ? g - 1 : 0
        } function r(c, b) { for (var g = l(c, b.x + 1) + 1; g--;) { var e; if (e = c[g].xMax >= b.x) e = c[g], e = b.x <= e.xMax && b.x >= e.xMin && b.y <= e.yMax && b.y >= e.yMin; if (e) return g } return -1 } function t(c) { var b = []; if (c.length) { b.push(["M", c[0].start.x, c[0].start.y]); for (var g = 0; g < c.length; ++g)b.push(["L", c[g].end.x, c[g].end.y]) } return b } function k(c, b) {
            c.yMin = v(c.yMin, b.yMin); c.yMax = w(c.yMax, b.yMax); c.xMin = v(c.xMin, b.xMin); c.xMax = w(c.xMax,
                b.xMax)
        } var u = e.extend, C = e.pick, w = Math.min, v = Math.max, z = Math.abs; return {
            straight: function (c, b) { return { path: [["M", c.x, c.y], ["L", b.x, b.y]], obstacles: [{ start: c, end: b }] } }, simpleConnect: u(function (c, b, g) {
                function e(c, a, d, f, B) { c = { x: c.x, y: c.y }; c[a] = d[f || a] + (B || 0); return c } function l(c, a, d) { var f = z(a[d] - c[d + "Min"]) > z(a[d] - c[d + "Max"]); return e(a, d, c, d + (f ? "Max" : "Min"), f ? 1 : -1) } var p = [], k = C(g.startDirectionX, z(b.x - c.x) > z(b.y - c.y)) ? "x" : "y", u = g.chartObstacles, w = r(u, c); g = r(u, b); if (-1 < g) {
                    var v = u[g]; g = l(v, b, k); v =
                        { start: g, end: b }; var D = g
                } else D = b; -1 < w && (u = u[w], g = l(u, c, k), p.push({ start: c, end: g }), g[k] >= c[k] === g[k] >= D[k] && (k = "y" === k ? "x" : "y", b = c[k] < b[k], p.push({ start: g, end: e(g, k, u, k + (b ? "Max" : "Min"), b ? 1 : -1) }), k = "y" === k ? "x" : "y")); c = p.length ? p[p.length - 1].end : c; g = e(c, k, D); p.push({ start: c, end: g }); k = e(g, "y" === k ? "x" : "y", D); p.push({ start: g, end: k }); p.push(v); return { path: t(p), obstacles: p }
            }, { requiresObstacles: !0 }), fastAvoid: u(function (c, b, g) {
                function e(a, d, f) {
                    var h, c = a.x < d.x ? 1 : -1; if (a.x < d.x) { var n = a; var B = d } else n = d, B = a;
                    if (a.y < d.y) { var x = a; var b = d } else x = d, b = a; for (h = 0 > c ? w(l(m, B.x), m.length - 1) : 0; m[h] && (0 < c && m[h].xMin <= B.x || 0 > c && m[h].xMax >= n.x);) { if (m[h].xMin <= B.x && m[h].xMax >= n.x && m[h].yMin <= b.y && m[h].yMax >= x.y) return f ? { y: a.y, x: a.x < d.x ? m[h].xMin - 1 : m[h].xMax + 1, obstacle: m[h] } : { x: a.x, y: a.y < d.y ? m[h].yMin - 1 : m[h].yMax + 1, obstacle: m[h] }; h += c } return d
                } function u(a, d, f, m, c) {
                    var h = c.soft, B = c.hard, b = m ? "x" : "y", n = { x: d.x, y: d.y }, q = { x: d.x, y: d.y }; c = a[b + "Max"] >= h[b + "Max"]; h = a[b + "Min"] <= h[b + "Min"]; var E = a[b + "Max"] >= B[b + "Max"]; B = a[b +
                        "Min"] <= B[b + "Min"]; var g = z(a[b + "Min"] - d[b]), x = z(a[b + "Max"] - d[b]); f = 10 > z(g - x) ? d[b] < f[b] : x < g; q[b] = a[b + "Min"]; n[b] = a[b + "Max"]; a = e(d, q, m)[b] !== q[b]; d = e(d, n, m)[b] !== n[b]; f = a ? d ? f : !0 : d ? !1 : f; f = h ? c ? f : !0 : c ? !1 : f; return B ? E ? f : !0 : E ? !1 : f
                } function p(h, b, c) {
                    if (h.x === b.x && h.y === b.y) return []; var q = c ? "x" : "y", E = g.obstacleOptions.margin; var n = { soft: { xMin: a, xMax: d, yMin: f, yMax: B }, hard: g.hardBounds }; var x = r(m, h); if (-1 < x) {
                        x = m[x]; n = u(x, h, b, c, n); k(x, g.hardBounds); var l = c ? { y: h.y, x: x[n ? "xMax" : "xMin"] + (n ? 1 : -1) } : {
                            x: h.x, y: x[n ? "yMax" :
                                "yMin"] + (n ? 1 : -1)
                        }; var y = r(m, l); -1 < y && (y = m[y], k(y, g.hardBounds), l[q] = n ? v(x[q + "Max"] - E + 1, (y[q + "Min"] + x[q + "Max"]) / 2) : w(x[q + "Min"] + E - 1, (y[q + "Max"] + x[q + "Min"]) / 2), h.x === l.x && h.y === l.y ? (D && (l[q] = n ? v(x[q + "Max"], y[q + "Max"]) + 1 : w(x[q + "Min"], y[q + "Min"]) - 1), D = !D) : D = !1); h = [{ start: h, end: l }]
                    } else q = e(h, { x: c ? b.x : h.x, y: c ? h.y : b.y }, c), h = [{ start: h, end: { x: q.x, y: q.y } }], q[c ? "x" : "y"] !== b[c ? "x" : "y"] && (n = u(q.obstacle, q, b, !c, n), k(q.obstacle, g.hardBounds), n = {
                        x: c ? q.x : q.obstacle[n ? "xMax" : "xMin"] + (n ? 1 : -1), y: c ? q.obstacle[n ? "yMax" :
                            "yMin"] + (n ? 1 : -1) : q.y
                    }, c = !c, h = h.concat(p({ x: q.x, y: q.y }, n, c))); return h = h.concat(p(h[h.length - 1].end, b, !c))
                } function K(a, d, f) { var c = w(a.xMax - d.x, d.x - a.xMin) < w(a.yMax - d.y, d.y - a.yMin); f = u(a, d, f, c, { soft: g.hardBounds, hard: g.hardBounds }); return c ? { y: d.y, x: a[f ? "xMax" : "xMin"] + (f ? 1 : -1) } : { x: d.x, y: a[f ? "yMax" : "yMin"] + (f ? 1 : -1) } } var G = C(g.startDirectionX, z(b.x - c.x) > z(b.y - c.y)), F = G ? "x" : "y", H = [], D = !1, A = g.obstacleMetrics, a = w(c.x, b.x) - A.maxWidth - 10, d = v(c.x, b.x) + A.maxWidth + 10, f = w(c.y, b.y) - A.maxHeight - 10, B = v(c.y, b.y) +
                    A.maxHeight + 10, m = g.chartObstacles; var E = l(m, a); A = l(m, d); m = m.slice(E, A + 1); if (-1 < (A = r(m, b))) { var y = K(m[A], b, c); H.push({ end: b, start: y }); b = y } for (; -1 < (A = r(m, b));)E = 0 > b[F] - c[F], y = { x: b.x, y: b.y }, y[F] = m[A][E ? F + "Max" : F + "Min"] + (E ? 1 : -1), H.push({ end: b, start: y }), b = y; c = p(c, b, G); c = c.concat(H.reverse()); return { path: t(c), obstacles: c }
            }, { requiresObstacles: !0 })
        }
    }); C(e, "parts-gantt/ArrowSymbols.js", [e["parts/SVGRenderer.js"]], function (e) {
        e.prototype.symbols.arrow = function (e, r, t, k) {
            return [["M", e, r + k / 2], ["L", e + t, r], ["L",
                e, r + k / 2], ["L", e + t, r + k]]
        }; e.prototype.symbols["arrow-half"] = function (l, r, t, k) { return e.prototype.symbols.arrow(l, r, t / 2, k) }; e.prototype.symbols["triangle-left"] = function (e, r, t, k) { return [["M", e + t, r], ["L", e, r + k / 2], ["L", e + t, r + k], ["Z"]] }; e.prototype.symbols["arrow-filled"] = e.prototype.symbols["triangle-left"]; e.prototype.symbols["triangle-left-half"] = function (l, r, t, k) { return e.prototype.symbols["triangle-left"](l, r, t / 2, k) }; e.prototype.symbols["arrow-filled-half"] = e.prototype.symbols["triangle-left-half"]
    });
    C(e, "parts-gantt/Pathfinder.js", [e["parts/Chart.js"], e["parts/Globals.js"], e["parts/Options.js"], e["parts/Point.js"], e["parts/Utilities.js"], e["parts-gantt/PathfinderAlgorithms.js"]], function (e, l, r, t, k, C) {
        function u(a) { var d = a.shapeArgs; return d ? { xMin: d.x, xMax: d.x + d.width, yMin: d.y, yMax: d.y + d.height } : (d = a.graphic && a.graphic.getBBox()) ? { xMin: a.plotX - d.width / 2, xMax: a.plotX + d.width / 2, yMin: a.plotY - d.height / 2, yMax: a.plotY + d.height / 2 } : null } function w(a) {
            for (var d = a.length, f = 0, c, m, b = [], e = function (a, d, f) {
                f =
                G(f, 10); var c = a.yMax + f > d.yMin - f && a.yMin - f < d.yMax + f, b = a.xMax + f > d.xMin - f && a.xMin - f < d.xMax + f, m = c ? a.xMin > d.xMax ? a.xMin - d.xMax : d.xMin - a.xMax : Infinity, h = b ? a.yMin > d.yMax ? a.yMin - d.yMax : d.yMin - a.yMax : Infinity; return b && c ? f ? e(a, d, Math.floor(f / 2)) : Infinity : A(m, h)
            }; f < d; ++f)for (c = f + 1; c < d; ++c)m = e(a[f], a[c]), 80 > m && b.push(m); b.push(80); return D(Math.floor(b.sort(function (a, d) { return a - d })[Math.floor(b.length / 10)] / 2 - 1), 1)
        } function v(a, d, f) { this.init(a, d, f) } function z(a) { this.init(a) } function c(a) {
            if (a.options.pathfinder ||
                a.series.reduce(function (a, f) { f.options && p(!0, f.options.connectors = f.options.connectors || {}, f.options.pathfinder); return a || f.options && f.options.pathfinder }, !1)) p(!0, a.options.connectors = a.options.connectors || {}, a.options.pathfinder), I('WARNING: Pathfinder options have been renamed. Use "chart.connectors" or "series.connectors" instead.')
        } ""; var b = k.addEvent, g = k.defined, I = k.error, J = k.extend, p = k.merge, L = k.objectEach, G = k.pick, F = k.splat, H = l.deg2rad, D = Math.max, A = Math.min; J(r.defaultOptions, {
            connectors: {
                type: "straight",
                lineWidth: 1, marker: { enabled: !1, align: "center", verticalAlign: "middle", inside: !1, lineWidth: 1 }, startMarker: { symbol: "diamond" }, endMarker: { symbol: "arrow-filled" }
            }
        }); v.prototype = {
            init: function (a, d, f) { this.fromPoint = a; this.toPoint = d; this.options = f; this.chart = a.series.chart; this.pathfinder = this.chart.pathfinder }, renderPath: function (a, d, f) {
                var c = this.chart, b = c.styledMode, e = c.pathfinder, g = !c.options.chart.forExport && !1 !== f, h = this.graphics && this.graphics.path; e.group || (e.group = c.renderer.g().addClass("highcharts-pathfinder-group").attr({ zIndex: -1 }).add(c.seriesGroup));
                e.group.translate(c.plotLeft, c.plotTop); h && h.renderer || (h = c.renderer.path().add(e.group), b || h.attr({ opacity: 0 })); h.attr(d); a = { d: a }; b || (a.opacity = 1); h[g ? "animate" : "attr"](a, f); this.graphics = this.graphics || {}; this.graphics.path = h
            }, addMarker: function (a, d, f) {
                var c = this.fromPoint.series.chart, b = c.pathfinder; c = c.renderer; var e = "start" === a ? this.fromPoint : this.toPoint, g = e.getPathfinderAnchorPoint(d); if (d.enabled && ((f = "start" === a ? f[1] : f[f.length - 2]) && "M" === f[0] || "L" === f[0])) {
                    f = { x: f[1], y: f[2] }; f = e.getRadiansToVector(f,
                        g); g = e.getMarkerVector(f, d.radius, g); f = -f / H; if (d.width && d.height) { var h = d.width; var k = d.height } else h = k = 2 * d.radius; this.graphics = this.graphics || {}; g = { x: g.x - h / 2, y: g.y - k / 2, width: h, height: k, rotation: f, rotationOriginX: g.x, rotationOriginY: g.y }; this.graphics[a] ? this.graphics[a].animate(g) : (this.graphics[a] = c.symbol(d.symbol).addClass("highcharts-point-connecting-path-" + a + "-marker").attr(g).add(b.group), c.styledMode || this.graphics[a].attr({
                            fill: d.color || this.fromPoint.color, stroke: d.lineColor, "stroke-width": d.lineWidth,
                            opacity: 0
                        }).animate({ opacity: 1 }, e.series.options.animation))
                }
            }, getPath: function (a) {
                var d = this.pathfinder, f = this.chart, c = d.algorithms[a.type], b = d.chartObstacles; if ("function" !== typeof c) I('"' + a.type + '" is not a Pathfinder algorithm.'); else return c.requiresObstacles && !b && (b = d.chartObstacles = d.getChartObstacles(a), f.options.connectors.algorithmMargin = a.algorithmMargin, d.chartObstacleMetrics = d.getObstacleMetrics(b)), c(this.fromPoint.getPathfinderAnchorPoint(a.startMarker), this.toPoint.getPathfinderAnchorPoint(a.endMarker),
                    p({ chartObstacles: b, lineObstacles: d.lineObstacles || [], obstacleMetrics: d.chartObstacleMetrics, hardBounds: { xMin: 0, xMax: f.plotWidth, yMin: 0, yMax: f.plotHeight }, obstacleOptions: { margin: a.algorithmMargin }, startDirectionX: d.getAlgorithmStartDirection(a.startMarker) }, a))
            }, render: function () {
                var a = this.fromPoint, d = a.series, f = d.chart, c = f.pathfinder, b = p(f.options.connectors, d.options.connectors, a.options.connectors, this.options), e = {}; f.styledMode || (e.stroke = b.lineColor || a.color, e["stroke-width"] = b.lineWidth, b.dashStyle &&
                    (e.dashstyle = b.dashStyle)); e["class"] = "highcharts-point-connecting-path highcharts-color-" + a.colorIndex; b = p(e, b); g(b.marker.radius) || (b.marker.radius = A(D(Math.ceil((b.algorithmMargin || 8) / 2) - 1, 1), 5)); a = this.getPath(b); f = a.path; a.obstacles && (c.lineObstacles = c.lineObstacles || [], c.lineObstacles = c.lineObstacles.concat(a.obstacles)); this.renderPath(f, e, d.options.animation); this.addMarker("start", p(b.marker, b.startMarker), f); this.addMarker("end", p(b.marker, b.endMarker), f)
            }, destroy: function () {
                this.graphics &&
                (L(this.graphics, function (a) { a.destroy() }), delete this.graphics)
            }
        }; z.prototype = {
            algorithms: C, init: function (a) { this.chart = a; this.connections = []; b(a, "redraw", function () { this.pathfinder.update() }) }, update: function (a) {
                var d = this.chart, c = this, b = c.connections; c.connections = []; d.series.forEach(function (a) {
                    a.visible && !a.options.isInternal && a.points.forEach(function (a) {
                        var b, f = a.options && a.options.connect && F(a.options.connect); a.visible && !1 !== a.isInside && f && f.forEach(function (f) {
                            b = d.get("string" === typeof f ?
                                f : f.to); b instanceof t && b.series.visible && b.visible && !1 !== b.isInside && c.connections.push(new v(a, b, "string" === typeof f ? {} : f))
                        })
                    })
                }); for (var e = 0, g, k, h = b.length, l = c.connections.length; e < h; ++e) { k = !1; for (g = 0; g < l; ++g)if (b[e].fromPoint === c.connections[g].fromPoint && b[e].toPoint === c.connections[g].toPoint) { c.connections[g].graphics = b[e].graphics; k = !0; break } k || b[e].destroy() } delete this.chartObstacles; delete this.lineObstacles; c.renderConnections(a)
            }, renderConnections: function (a) {
                a ? this.chart.series.forEach(function (a) {
                    var d =
                        function () { var d = a.chart.pathfinder; (d && d.connections || []).forEach(function (d) { d.fromPoint && d.fromPoint.series === a && d.render() }); a.pathfinderRemoveRenderEvent && (a.pathfinderRemoveRenderEvent(), delete a.pathfinderRemoveRenderEvent) }; !1 === a.options.animation ? d() : a.pathfinderRemoveRenderEvent = b(a, "afterAnimate", d)
                }) : this.connections.forEach(function (a) { a.render() })
            }, getChartObstacles: function (a) {
                for (var d = [], c = this.chart.series, b = G(a.algorithmMargin, 0), e, k = 0, l = c.length; k < l; ++k)if (c[k].visible && !c[k].options.isInternal) for (var h =
                    0, p = c[k].points.length, n; h < p; ++h)n = c[k].points[h], n.visible && (n = u(n)) && d.push({ xMin: n.xMin - b, xMax: n.xMax + b, yMin: n.yMin - b, yMax: n.yMax + b }); d = d.sort(function (a, d) { return a.xMin - d.xMin }); g(a.algorithmMargin) || (e = a.algorithmMargin = w(d), d.forEach(function (a) { a.xMin -= e; a.xMax += e; a.yMin -= e; a.yMax += e })); return d
            }, getObstacleMetrics: function (a) { for (var d = 0, c = 0, b, e, g = a.length; g--;)b = a[g].xMax - a[g].xMin, e = a[g].yMax - a[g].yMin, d < b && (d = b), c < e && (c = e); return { maxHeight: c, maxWidth: d } }, getAlgorithmStartDirection: function (a) {
                var d =
                    "top" !== a.verticalAlign && "bottom" !== a.verticalAlign; return "left" !== a.align && "right" !== a.align ? d ? void 0 : !1 : d ? !0 : void 0
            }
        }; l.Connection = v; l.Pathfinder = z; J(t.prototype, {
            getPathfinderAnchorPoint: function (a) { var d = u(this); switch (a.align) { case "right": var c = "xMax"; break; case "left": c = "xMin" }switch (a.verticalAlign) { case "top": var b = "yMin"; break; case "bottom": b = "yMax" }return { x: c ? d[c] : (d.xMin + d.xMax) / 2, y: b ? d[b] : (d.yMin + d.yMax) / 2 } }, getRadiansToVector: function (a, c) {
                var b; g(c) || (b = u(this)) && (c = {
                    x: (b.xMin + b.xMax) /
                        2, y: (b.yMin + b.yMax) / 2
                }); return Math.atan2(c.y - a.y, a.x - c.x)
            }, getMarkerVector: function (a, c, b) { var d = 2 * Math.PI, e = u(this), f = e.xMax - e.xMin, g = e.yMax - e.yMin, h = Math.atan2(g, f), k = !1; f /= 2; var n = g / 2, l = e.xMin + f; e = e.yMin + n; for (var p = l, r = e, t = {}, v = 1, w = 1; a < -Math.PI;)a += d; for (; a > Math.PI;)a -= d; d = Math.tan(a); a > -h && a <= h ? (w = -1, k = !0) : a > h && a <= Math.PI - h ? w = -1 : a > Math.PI - h || a <= -(Math.PI - h) ? (v = -1, k = !0) : v = -1; k ? (p += v * f, r += w * f * d) : (p += g / (2 * d) * v, r += w * n); b.x !== l && (p = b.x); b.y !== e && (r = b.y); t.x = p + c * Math.cos(a); t.y = r - c * Math.sin(a); return t }
        });
        e.prototype.callbacks.push(function (a) { !1 !== a.options.connectors.enabled && (c(a), this.pathfinder = new z(this), this.pathfinder.update(!0)) })
    }); C(e, "masters/modules/pathfinder.src.js", [], function () { })
});
//# sourceMappingURL=pathfinder.js.map