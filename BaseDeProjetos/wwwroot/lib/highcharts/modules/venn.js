/*
 Highcharts JS v8.1.2 (2020-06-16)

 (c) 2017-2019 Highsoft AS
 Authors: Jon Arild Nygard

 License: www.highcharts.com/license
*/
(function (a) { "object" === typeof module && module.exports ? (a["default"] = a, module.exports = a) : "function" === typeof define && define.amd ? define("highcharts/modules/venn", ["highcharts"], function (m) { a(m); a.Highcharts = m; return a }) : a("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (a) {
    function m(a, f, b, B) { a.hasOwnProperty(f) || (a[f] = B.apply(null, b)) } a = a ? a._modules : {}; m(a, "mixins/draw-point.js", [], function () {
        var a = function (f) {
            var b, a = this, k = a.graphic, e = f.animatableAttribs, u = f.onComplete, w = f.css, m = f.renderer,
            r = null === (b = a.series) || void 0 === b ? void 0 : b.options.animation; if (a.shouldDraw()) k || (a.graphic = k = m[f.shapeType](f.shapeArgs).add(f.group)), k.css(w).attr(f.attribs).animate(e, f.isNew ? !1 : r, u); else if (k) { var n = function () { a.graphic = k = k.destroy(); "function" === typeof u && u() }; Object.keys(e).length ? k.animate(e, void 0, function () { n() }) : n() }
        }; return function (f) { (f.attribs = f.attribs || {})["class"] = this.getClassName(); a.call(this, f) }
    }); m(a, "mixins/geometry.js", [], function () {
        return {
            getAngleBetweenPoints: function (a,
                f) { return Math.atan2(f.x - a.x, f.y - a.y) }, getCenterOfPoints: function (a) { var f = a.reduce(function (b, f) { b.x += f.x; b.y += f.y; return b }, { x: 0, y: 0 }); return { x: f.x / a.length, y: f.y / a.length } }, getDistanceBetweenPoints: function (a, f) { return Math.sqrt(Math.pow(f.x - a.x, 2) + Math.pow(f.y - a.y, 2)) }
        }
    }); m(a, "mixins/geometry-circles.js", [a["mixins/geometry.js"]], function (a) {
        function f(d, c) { c = Math.pow(10, c); return Math.round(d * c) / c } function b(d) {
            if (0 >= d) throw Error("radius of circle must be a positive number."); return Math.PI *
                d * d
        } function k(d, c) { return d * d * Math.acos(1 - c / d) - (d - c) * Math.sqrt(c * (2 * d - c)) } function m(d, c) { var a = t(d, c), b = d.r, e = c.r, z = []; if (a < b + e && a > Math.abs(b - e)) { b *= b; var x = (b - e * e + a * a) / (2 * a); e = Math.sqrt(b - x * x); b = d.x; z = c.x; d = d.y; var k = c.y; c = b + x * (z - b) / a; x = d + x * (k - d) / a; d = e / a * -(k - d); a = e / a * -(z - b); z = [{ x: f(c + d, 14), y: f(x - a, 14) }, { x: f(c - d, 14), y: f(x + a, 14) }] } return z } function e(d) {
            return d.reduce(function (d, a, b, f) {
                f = f.slice(b + 1).reduce(function (d, c, f) { var e = [b, f + b + 1]; return d.concat(m(a, c).map(function (d) { d.indexes = e; return d })) },
                    []); return d.concat(f)
            }, [])
        } function u(d, c) { return t(d, c) <= c.r + 1e-10 } function w(d, c) { return !c.some(function (c) { return !u(d, c) }) } function y(d) { return e(d).filter(function (c) { return w(c, d) }) } var r = a.getAngleBetweenPoints, n = a.getCenterOfPoints, t = a.getDistanceBetweenPoints; return {
            getAreaOfCircle: b, getAreaOfIntersectionBetweenCircles: function (d) {
                var c = y(d); if (1 < c.length) {
                    var a = n(c); c = c.map(function (d) { d.angle = r(a, d); return d }).sort(function (d, c) { return c.angle - d.angle }); var b = c[c.length - 1]; c = c.reduce(function (c,
                        b) { var a = c.startPoint, f = n([a, b]), e = b.indexes.filter(function (d) { return -1 < a.indexes.indexOf(d) }).reduce(function (c, e) { e = d[e]; var q = r(e, b), k = r(e, a); q = k - (k - q + (k < q ? 2 * Math.PI : 0)) / 2; q = t(f, { x: e.x + e.r * Math.sin(q), y: e.y + e.r * Math.cos(q) }); e = e.r; q > 2 * e && (q = 2 * e); if (!c || c.width > q) c = { r: e, largeArc: q > e ? 1 : 0, width: q, x: b.x, y: b.y }; return c }, null); if (e) { var q = e.r; c.arcs.push(["A", q, q, 0, e.largeArc, 1, e.x, e.y]); c.startPoint = b } return c }, { startPoint: b, arcs: [] }).arcs; if (0 !== c.length && 1 !== c.length) {
                            c.unshift(["M", b.x, b.y]); var f =
                                { center: a, d: c }
                        }
                } return f
            }, getCircleCircleIntersection: m, getCirclesIntersectionPoints: e, getCirclesIntersectionPolygon: y, getCircularSegmentArea: k, getOverlapBetweenCircles: function (d, c, a) { var e = 0; a < d + c && (a <= Math.abs(c - d) ? e = b(d < c ? d : c) : (e = (d * d - c * c + a * a) / (2 * a), a -= e, e = k(d, d - e) + k(c, c - a)), e = f(e, 14)); return e }, isCircle1CompletelyOverlappingCircle2: function (d, c) { return t(d, c) + c.r < d.r + 1e-10 }, isPointInsideCircle: u, isPointInsideAllCircles: w, isPointOutsideAllCircles: function (d, c) {
                return !c.some(function (c) {
                    return u(d,
                        c)
                })
            }, round: f
        }
    }); m(a, "mixins/nelder-mead.js", [], function () {
        var a = function (a) { a = a.slice(0, -1); for (var b = a.length, f = [], k = function (a, b) { a.sum += b[a.i]; return a }, e = 0; e < b; e++)f[e] = a.reduce(k, { sum: 0, i: e }).sum / b; return f }; return {
            getCentroid: a, nelderMead: function (f, b) {
                var k = function (a, c) { return a.fx - c.fx }, m = function (a, c, b, e) { return c.map(function (c, d) { return a * c + b * e[d] }) }, e = function (a, c) { c.fx = f(c); a[a.length - 1] = c; return a }, u = function (a) { var c = a[0]; return a.map(function (a) { a = m(.5, c, .5, a); a.fx = f(a); return a }) },
                w = function (a, c, b, e) { a = m(b, a, e, c); a.fx = f(a); return a }; b = function (a) { var c = a.length, b = Array(c + 1); b[0] = a; b[0].fx = f(a); for (var d = 0; d < c; ++d) { var e = a.slice(); e[d] = e[d] ? 1.05 * e[d] : .001; e.fx = f(e); b[d + 1] = e } return b }(b); for (var y = 0; 100 > y; y++) { b.sort(k); var r = b[b.length - 1], n = a(b), t = w(n, r, 2, -1); t.fx < b[0].fx ? (r = w(n, r, 3, -2), b = e(b, r.fx < t.fx ? r : t)) : t.fx >= b[b.length - 2].fx ? t.fx > r.fx ? (n = w(n, r, .5, .5), b = n.fx < r.fx ? e(b, n) : u(b)) : (n = w(n, r, 1.5, -.5), b = n.fx < t.fx ? e(b, n) : u(b)) : b = e(b, t) } return b[0]
            }
        }
    }); m(a, "modules/venn.src.js",
        [a["parts/Color.js"], a["parts/Globals.js"], a["parts/Utilities.js"], a["mixins/draw-point.js"], a["mixins/geometry.js"], a["mixins/geometry-circles.js"], a["mixins/nelder-mead.js"]], function (a, f, b, m, C, e, u) {
            function k(a, c) {
                var l = a.sets, b = c.reduce(function (a, c) { var b = -1 < l.indexOf(c.sets[0]); a[b ? "internal" : "external"].push(c.circle); return a }, { internal: [], external: [] }); b.external = b.external.filter(function (a) { return b.internal.some(function (c) { return !Y(a, c) }) }); a = Z(b.internal, b.external); c = L(a, b.internal,
                    b.external); return { position: a, width: c }
            } function y(a) { var c = {}, b = {}; if (0 < a.length) { var l = M(a), g = a.filter(A); a.forEach(function (a) { var p = a.sets, d = p.join(); if (p = A(a) ? l[d] : x(p.map(function (a) { return l[a] }))) c[d] = p, b[d] = k(a, g) }) } return { mapOfIdToShape: c, mapOfIdToLabelValues: b } } var r = a.parse; a = b.addEvent; var n = b.animObject, t = b.extend, d = b.isArray, c = b.isNumber, q = b.isObject, B = b.isString, H = b.merge; b = b.seriesType; var z = e.getAreaOfCircle, x = e.getAreaOfIntersectionBetweenCircles, U = e.getCircleCircleIntersection,
                V = e.getCirclesIntersectionPolygon, I = e.getOverlapBetweenCircles, Y = e.isCircle1CompletelyOverlappingCircle2, J = e.isPointInsideAllCircles, W = e.isPointInsideCircle, K = e.isPointOutsideAllCircles, X = u.nelderMead, aa = C.getCenterOfPoints, E = C.getDistanceBetweenPoints, N = f.seriesTypes, ba = function (a) { return Object.keys(a).map(function (c) { return a[c] }) }, ca = function (a) { var c = 0; 2 === a.length && (c = a[0], a = a[1], c = I(c.r, a.r, E(c, a))); return c }, O = function (a, c) {
                    return c.reduce(function (c, b) {
                        var g = 0; 1 < b.sets.length && (g = b.value,
                            b = ca(b.sets.map(function (c) { return a[c] })), b = g - b, g = Math.round(b * b * 1E11) / 1E11); return c + g
                    }, 0)
                }, P = function (a, c, b, d, g) { var l = a(c), e = a(b); g = g || 100; d = d || 1e-10; var p = b - c, f = 1; if (c >= b) throw Error("a must be smaller than b."); if (0 < l * e) throw Error("f(a) and f(b) must have opposite signs."); if (0 === l) var h = c; else if (0 === e) h = b; else for (; f++ <= g && 0 !== v && p > d;) { p = (b - c) / 2; h = c + p; var v = a(h); 0 < l * v ? c = h : b = h } return h }, F = function (a, c, b) { var d = a + c; return 0 >= b ? d : z(a < c ? a : c) <= b ? 0 : P(function (g) { g = I(a, c, g); return b - g }, 0, d) }, A =
                    function (a) { return d(a.sets) && 1 === a.sets.length }, G = function (a, c, b) { c = c.reduce(function (c, b) { b = b.r - E(a, b); return b <= c ? b : c }, Number.MAX_VALUE); return c = b.reduce(function (c, b) { b = E(a, b) - b.r; return b <= c ? b : c }, c) }, Z = function (a, c) {
                        var b = a.reduce(function (b, d) { var e = d.r / 2; return [{ x: d.x, y: d.y }, { x: d.x + e, y: d.y }, { x: d.x - e, y: d.y }, { x: d.x, y: d.y + e }, { x: d.x, y: d.y - e }].reduce(function (b, d) { var e = G(d, a, c); b.margin < e && (b.point = d, b.margin = e); return b }, b) }, { point: void 0, margin: -Number.MAX_VALUE }).point; b = X(function (b) {
                            return -G({
                                x: b[0],
                                y: b[1]
                            }, a, c)
                        }, [b.x, b.y]); b = { x: b[0], y: b[1] }; J(b, a) && K(b, c) || (b = 1 < a.length ? aa(V(a)) : { x: a[0].x, y: a[0].y }); return b
                    }, L = function (a, b, c) { var d = b.reduce(function (a, b) { return Math.min(b.r, a) }, Infinity), e = c.filter(function (b) { return !W(a, b) }); c = function (c, d) { return P(function (g) { var l = { x: a.x + d * g, y: a.y }; l = J(l, b) && K(l, e); return -(c - g) + (l ? 0 : Number.MAX_VALUE) }, 0, c) }; return 2 * Math.min(c(d, -1), c(d, 1)) }, Q = function (a) {
                        var b = a.filter(function (a) { return 2 === a.sets.length }).reduce(function (a, b) {
                            b.sets.forEach(function (c,
                                d, e) { q(a[c]) || (a[c] = { overlapping: {}, totalOverlap: 0 }); a[c].totalOverlap += b.value; a[c].overlapping[e[1 - d]] = b.value }); return a
                        }, {}); a.filter(A).forEach(function (a) { t(a, b[a.sets[0]]) }); return a
                    }, R = function (a, b) { return b.totalOverlap - a.totalOverlap }, M = function (a) {
                        var b = [], c = {}; a.filter(function (a) { return 1 === a.sets.length }).forEach(function (a) { c[a.sets[0]] = a.circle = { x: Number.MAX_VALUE, y: Number.MAX_VALUE, r: Math.sqrt(a.value / Math.PI) } }); var d = function (a, c) { var d = a.circle; d.x = c.x; d.y = c.y; b.push(a) }; Q(a);
                        var e = a.filter(A).sort(R); d(e.shift(), { x: 0, y: 0 }); var f = a.filter(function (a) { return 2 === a.sets.length }); e.forEach(function (a) {
                            var e = a.circle, g = e.r, l = a.overlapping, p = b.reduce(function (a, d, p) {
                                var h = d.circle, v = F(g, h.r, l[d.sets[0]]), D = [{ x: h.x + v, y: h.y }, { x: h.x - v, y: h.y }, { x: h.x, y: h.y + v }, { x: h.x, y: h.y - v }]; b.slice(p + 1).forEach(function (a) { var b = a.circle; a = F(g, b.r, l[a.sets[0]]); D = D.concat(U({ x: h.x, y: h.y, r: v }, { x: b.x, y: b.y, r: a })) }); D.forEach(function (b) {
                                    e.x = b.x; e.y = b.y; var d = O(c, f); d < a.loss && (a.loss = d, a.coordinates =
                                        b)
                                }); return a
                            }, { loss: Number.MAX_VALUE, coordinates: void 0 }); d(a, p.coordinates)
                        }); return c
                    }, S = function (a) { var b = {}; return q(a) && c(a.value) && -1 < a.value && d(a.sets) && 0 < a.sets.length && !a.sets.some(function (a) { var c = !1; !b[a] && B(a) ? b[a] = !0 : c = !0; return c }) }, T = function (a) {
                        a = d(a) ? a : []; var b = a.reduce(function (a, b) { S(b) && A(b) && 0 < b.value && -1 === a.indexOf(b.sets[0]) && a.push(b.sets[0]); return a }, []).sort(), c = a.reduce(function (a, c) {
                            S(c) && !c.sets.some(function (a) { return -1 === b.indexOf(a) }) && (a[c.sets.sort().join()] =
                                c); return a
                        }, {}); b.reduce(function (a, b, c, d) { d.slice(c + 1).forEach(function (c) { a.push(b + "," + c) }); return a }, []).forEach(function (a) { if (!c[a]) { var b = { sets: a.split(","), value: 0 }; c[a] = b } }); return ba(c)
                    }, da = function (a, b, c) { var d = c.bottom - c.top, e = c.right - c.left; d = Math.min(0 < e ? 1 / e * a : 1, 0 < d ? 1 / d * b : 1); return { scale: d, centerX: a / 2 - (c.right + c.left) / 2 * d, centerY: b / 2 - (c.top + c.bottom) / 2 * d } }; b("venn", "scatter", {
                        borderColor: "#cccccc", borderDashStyle: "solid", borderWidth: 1, brighten: 0, clip: !1, colorByPoint: !0, dataLabels: {
                            enabled: !0,
                            verticalAlign: "middle", formatter: function () { return this.point.name }
                        }, inactiveOtherPoints: !0, marker: !1, opacity: .75, showInLegend: !1, states: { hover: { opacity: 1, borderColor: "#333333" }, select: { color: "#cccccc", borderColor: "#000000", animation: !1 }, inactive: { opacity: .075 } }, tooltip: { pointFormat: "{point.name}: {point.value}" }
                    }, {
                        isCartesian: !1, axisTypes: [], directTouch: !0, pointArrayMap: ["value"], init: function () { N.scatter.prototype.init.apply(this, arguments); delete this.opacity }, translate: function () {
                            var a = this.chart;
                            this.processedXData = this.xData; this.generatePoints(); var b = T(this.options.data); b = y(b); var e = b.mapOfIdToShape, f = b.mapOfIdToLabelValues; b = Object.keys(e).filter(function (a) { return (a = e[a]) && c(a.r) }).reduce(function (a, b) { var d = e[b]; b = d.x - d.r; var f = d.x + d.r, g = d.y + d.r; d = d.y - d.r; if (!c(a.left) || a.left > b) a.left = b; if (!c(a.right) || a.right < f) a.right = f; if (!c(a.top) || a.top > d) a.top = d; if (!c(a.bottom) || a.bottom < g) a.bottom = g; return a }, { top: 0, bottom: 0, left: 0, right: 0 }); a = da(a.plotWidth, a.plotHeight, b); var g = a.scale, k =
                                a.centerX, m = a.centerY; this.points.forEach(function (a) {
                                    var b = d(a.sets) ? a.sets : [], l = b.join(), p = e[l], h = f[l] || {}; l = h.width; h = h.position; var v = a.options && a.options.dataLabels; if (p) { if (p.r) var n = { x: k + p.x * g, y: m + p.y * g, r: p.r * g }; else p.d && (p = p.d, p.forEach(function (a) { "M" === a[0] ? (a[1] = k + a[1] * g, a[2] = m + a[2] * g) : "A" === a[0] && (a[1] *= g, a[2] *= g, a[6] = k + a[6] * g, a[7] = m + a[7] * g) }), n = { d: p }); h ? (h.x = k + h.x * g, h.y = m + h.y * g) : h = {}; c(l) && (l = Math.round(l * g)) } a.shapeArgs = n; h && n && (a.plotX = h.x, a.plotY = h.y); l && n && (a.dlOptions = H(!0, { style: { width: l } },
                                        q(v) && v)); a.name = a.options.name || b.join("\u2229")
                                })
                        }, drawPoints: function () { var a = this, b = a.chart, c = a.group, e = b.renderer; (a.points || []).forEach(function (f) { var l = { zIndex: d(f.sets) ? f.sets.length : 0 }, g = f.shapeArgs; b.styledMode || t(l, a.pointAttribs(f, f.state)); f.draw({ isNew: !f.graphic, animatableAttribs: g, attribs: l, group: c, renderer: e, shapeType: g && g.d ? "path" : "circle" }) }) }, pointAttribs: function (a, b) {
                            var c = this.options || {}; a = H(c, { color: a && a.color }, a && a.options || {}, b && c.states[b] || {}); return {
                                fill: r(a.color).setOpacity(a.opacity).brighten(a.brightness).get(),
                                stroke: a.borderColor, "stroke-width": a.borderWidth, dashstyle: a.borderDashStyle
                            }
                        }, animate: function (a) { if (!a) { var b = n(this.options.animation); this.points.forEach(function (a) { var c = a.shapeArgs; if (a.graphic && c) { var d = {}, e = {}; c.d ? d.opacity = .001 : (d.r = 0, e.r = c.r); a.graphic.attr(d).animate(e, b); c.d && setTimeout(function () { a && a.graphic && a.graphic.animate({ opacity: 1 }) }, b.duration) } }, this) } }, utils: {
                            addOverlapToSets: Q, geometry: C, geometryCircles: e, getLabelWidth: L, getMarginFromCircles: G, getDistanceBetweenCirclesByOverlap: F,
                            layoutGreedyVenn: M, loss: O, nelderMead: u, processVennData: T, sortByTotalOverlap: R
                        }
                    }, { draw: m, shouldDraw: function () { return !!this.shapeArgs }, isValid: function () { return c(this.value) } }); a(N.venn, "afterSetOptions", function (a) { var b = a.options.states; this.is("venn") && Object.keys(b).forEach(function (a) { b[a].halo = !1 }) })
        }); m(a, "masters/modules/venn.src.js", [], function () { })
});
//# sourceMappingURL=venn.js.map