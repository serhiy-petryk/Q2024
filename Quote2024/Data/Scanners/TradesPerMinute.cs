﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Data.Scanners
{
    /* Result for 2023 (Turnover>50 mln, daily trade count>5000, minute open>=5.0f
Time	Count	Trades	% change	
09:30:00	256661	370		
09:31:00	238562	246	66,48648649	
09:32:00	241257	225	91,46341463	
09:33:00	241603	207	92	
09:34:00	242176	191	92,2705314	
09:35:00	245698	212	110,9947644	!!
09:36:00	245178	197	92,9245283	
09:37:00	245150	185	93,90862944	
09:38:00	245698	179	96,75675676	
09:39:00	245553	174	97,20670391	
09:40:00	248298	192	110,3448276	!!
09:41:00	246478	177	92,1875	
09:42:00	246483	173	97,74011299	
09:43:00	246039	164	94,79768786	
09:44:00	245827	159	96,95121951	
09:45:00	249465	188	118,2389937	!!
09:46:00	247050	175	93,08510638	
09:47:00	247658	175	100	
09:48:00	248377	173	98,85714286	
09:49:00	248965	171	98,84393064	
09:50:00	249237	178	104,0935673	
09:51:00	248566	169	94,94382022	
09:52:00	247720	161	95,26627219	
09:53:00	247924	158	98,13664596	
09:54:00	247100	154	97,46835443	
09:55:00	247246	157	101,9480519	
09:56:00	247337	153	97,4522293	
09:57:00	247581	150	98,03921569	
09:58:00	247431	142	94,66666667	
09:59:00	248780	143	100,7042254	
10:00:00	254896	211	147,5524476	!!!
10:01:00	251297	169	80,09478673	
10:02:00	250394	164	97,04142012	
10:03:00	250143	156	95,12195122	
10:04:00	250126	154	98,71794872	
10:05:00	251362	164	106,4935065	!
10:06:00	250914	159	96,95121951	
10:07:00	250643	151	94,96855346	
10:08:00	250560	149	98,67549669	
10:09:00	250516	147	98,65771812	
10:10:00	251089	153	104,0816327	
10:11:00	250272	148	96,73202614	
10:12:00	250518	147	99,32432432	
10:13:00	249776	140	95,23809524	
10:14:00	249673	138	98,57142857	
10:15:00	251171	149	107,9710145	!
10:16:00	250088	144	96,6442953	
10:17:00	250355	142	98,61111111	
10:18:00	249819	137	96,47887324	
10:19:00	249927	137	100	
10:20:00	249967	141	102,919708	
10:21:00	249893	137	97,16312057	
10:22:00	249800	134	97,81021898	
10:23:00	249668	131	97,76119403	
10:24:00	249491	131	100	
10:25:00	250210	136	103,8167939	
10:26:00	249419	131	96,32352941	
10:27:00	249702	130	99,23664122	
10:28:00	249522	128	98,46153846	
10:29:00	249313	126	98,4375	
10:30:00	252058	151	119,8412698	!!
10:31:00	249822	136	90,06622517	
10:32:00	249318	131	96,32352941	
10:33:00	249768	129	98,47328244	
10:34:00	250224	129	100	
10:35:00	250939	136	105,4263566	!
10:36:00	249994	130	95,58823529	
10:37:00	249656	126	96,92307692	
10:38:00	249592	123	97,61904762	
10:39:00	248466	119	96,74796748	
10:40:00	250080	128	107,5630252	!
10:41:00	248784	120	93,75	
10:42:00	249002	119	99,16666667	
10:43:00	248777	116	97,4789916	
10:44:00	248119	114	98,27586207	
10:45:00	249956	125	109,6491228	!
10:46:00	249192	123	98,4	
10:47:00	249141	120	97,56097561	
10:48:00	248404	116	96,66666667	
10:49:00	248809	115	99,13793103	
10:50:00	249637	120	104,3478261	
10:51:00	248714	117	97,5	
10:52:00	249062	114	97,43589744	
10:53:00	248958	111	97,36842105	
10:54:00	248798	110	99,0990991	
10:55:00	249862	116	105,4545455	!
10:56:00	249040	111	95,68965517	
10:57:00	248814	110	99,0990991	
10:58:00	249455	110	100	
10:59:00	249697	113	102,7272727	
11:00:00	250655	123	108,8495575	!
11:01:00	248171	110	89,43089431	
11:02:00	247831	108	98,18181818	
11:03:00	247084	104	96,2962963	
11:04:00	247731	104	100	
11:05:00	248062	109	104,8076923	
11:06:00	247293	106	97,24770642	
11:07:00	248170	106	100	
11:08:00	247581	104	98,11320755	
11:09:00	247237	103	99,03846154	
11:10:00	248031	108	104,8543689	
11:11:00	247137	103	95,37037037	
11:12:00	247474	104	100,9708738	
11:13:00	247535	102	98,07692308	
11:14:00	246568	98	96,07843137	
11:15:00	249234	111	113,2653061	!!
11:16:00	247456	103	92,79279279	
11:17:00	247190	102	99,02912621	
11:18:00	246595	98	96,07843137	
11:19:00	247667	100	102,0408163	
11:20:00	247913	103	103	
11:21:00	247375	101	98,05825243	
11:22:00	247029	99	98,01980198	
11:23:00	246966	98	98,98989899	
11:24:00	246895	97	98,97959184	
11:25:00	248819	104	107,2164948	!
11:26:00	247938	101	97,11538462	
11:27:00	247220	97	96,03960396	
11:28:00	247110	98	101,0309278	
11:29:00	246896	99	101,0204082	
11:30:00	249246	109	110,1010101	!!
11:31:00	247387	103	94,49541284	
11:32:00	246949	100	97,08737864	
11:33:00	246453	97	97	
11:34:00	247454	100	103,0927835	
11:35:00	247673	101	101	
11:36:00	246262	96	95,04950495	
11:37:00	245642	93	96,875	
11:38:00	245772	92	98,92473118	
11:39:00	245292	92	100	
11:40:00	245942	95	103,2608696	
11:41:00	245672	93	97,89473684	
11:42:00	245620	92	98,92473118	
11:43:00	245215	92	100	
11:44:00	244124	88	95,65217391	
11:45:00	246493	95	107,9545455	!
11:46:00	245265	92	96,84210526	
11:47:00	245385	91	98,91304348	
11:48:00	244789	88	96,7032967	
11:49:00	245239	89	101,1363636	
11:50:00	246224	93	104,494382	
11:51:00	245669	90	96,77419355	
11:52:00	245919	90	100	
11:53:00	244642	87	96,66666667	
11:54:00	244111	84	96,55172414	
11:55:00	245897	89	105,952381	!
11:56:00	245890	91	102,247191	
11:57:00	245197	86	94,50549451	
11:58:00	244875	84	97,6744186	
11:59:00	244645	85	101,1904762	
12:00:00	246672	98	115,2941176	!!
12:01:00	245217	93	94,89795918	
12:02:00	244554	88	94,62365591	
12:03:00	243864	85	96,59090909	
12:04:00	244621	85	100	
12:05:00	244679	87	102,3529412	
12:06:00	243643	86	98,85057471	
12:07:00	244195	85	98,8372093	
12:08:00	242856	82	96,47058824	
12:09:00	242718	81	98,7804878	
12:10:00	244182	86	106,1728395	!
12:11:00	243052	83	96,51162791	
12:12:00	243179	82	98,79518072	
12:13:00	242225	80	97,56097561	
12:14:00	242618	80	100	
12:15:00	245249	87	108,75	!
12:16:00	244505	84	96,55172414	
12:17:00	244403	82	97,61904762	
12:18:00	244328	82	100	
12:19:00	244066	81	98,7804878	
12:20:00	245075	85	104,9382716	
12:21:00	244217	83	97,64705882	
12:22:00	243650	80	96,38554217	
12:23:00	243329	79	98,75	
12:24:00	243152	79	100	
12:25:00	244639	83	105,0632911	!
12:26:00	243006	80	96,38554217	
12:27:00	243430	79	98,75	
12:28:00	243263	79	100	
12:29:00	241935	77	97,46835443	
12:30:00	245464	87	112,987013	!!
12:31:00	243792	82	94,25287356	
12:32:00	243406	80	97,56097561	
12:33:00	242858	78	97,5	
12:34:00	243768	79	101,2820513	
12:35:00	243508	81	102,5316456	
12:36:00	243061	79	97,5308642	
12:37:00	242989	79	100	
12:38:00	242729	76	96,20253165	
12:39:00	241438	75	98,68421053	
12:40:00	242846	79	105,3333333	!
12:41:00	242265	78	98,73417722	
12:42:00	241886	77	98,71794872	
12:43:00	241953	76	98,7012987	
12:44:00	241397	74	97,36842105	
12:45:00	244629	83	112,1621622	!!
12:46:00	241822	78	93,97590361	
12:47:00	242628	78	100	
12:48:00	241956	75	96,15384615	
12:49:00	242414	76	101,3333333	
12:50:00	243343	81	106,5789474	!
12:51:00	241955	77	95,0617284	
12:52:00	242486	76	98,7012987	
12:53:00	241740	75	98,68421053	
12:54:00	240716	74	98,66666667	
12:55:00	243545	79	106,7567568	!
12:56:00	241971	77	97,46835443	
12:57:00	242431	76	98,7012987	
12:58:00	242477	76	100	
12:59:00	242415	78	102,6315789	
13:00:00	243354	84	107,6923077	!
13:01:00	242934	88	104,7619048	
13:02:00	242489	86	97,72727273	
13:03:00	241265	78	90,69767442	
13:04:00	241343	77	98,71794872	
13:05:00	243197	84	109,0909091	!
13:06:00	242505	82	97,61904762	
13:07:00	241422	77	93,90243902	
13:08:00	240696	77	100	
13:09:00	240501	75	97,4025974	
13:10:00	241610	79	105,3333333	!
13:11:00	241181	78	98,73417722	
13:12:00	241494	78	100	
13:13:00	240863	75	96,15384615	
13:14:00	240386	76	101,3333333	
13:15:00	243809	85	111,8421053	!!
13:16:00	241972	80	94,11764706	
13:17:00	241732	79	98,75	
13:18:00	240838	76	96,20253165	
13:19:00	241513	75	98,68421053	
13:20:00	243261	82	109,3333333	!
13:21:00	242282	78	95,12195122	
13:22:00	242082	77	98,71794872	
13:23:00	240676	74	96,1038961	
13:24:00	240962	75	101,3513514	
13:25:00	242391	80	106,6666667	!
13:26:00	242050	78	97,5	
13:27:00	242029	76	97,43589744	
13:28:00	241555	77	101,3157895	
13:29:00	241368	75	97,4025974	
13:30:00	245835	91	121,3333333	!!!
13:31:00	242648	82	90,10989011	
13:32:00	242084	80	97,56097561	
13:33:00	243074	79	98,75	
13:34:00	243232	77	97,46835443	
13:35:00	244244	82	106,4935065	!
13:36:00	245317	82	100	
13:37:00	242894	78	95,12195122	
13:38:00	243227	76	97,43589744	
13:39:00	242682	75	98,68421053	
13:40:00	244590	83	110,6666667	!!
13:41:00	243357	80	96,38554217	
13:42:00	243356	79	98,75	
13:43:00	242007	75	94,93670886	
13:44:00	241440	74	98,66666667	
13:45:00	244261	81	109,4594595	!
13:46:00	242372	76	93,82716049	
13:47:00	242528	76	100	
13:48:00	242351	76	100	
13:49:00	242814	75	98,68421053	
13:50:00	243480	78	104	
13:51:00	243068	77	98,71794872	
13:52:00	242948	76	98,7012987	
13:53:00	242773	75	98,68421053	
13:54:00	241944	73	97,33333333	
13:55:00	244189	80	109,5890411	!
13:56:00	243154	77	96,25	
13:57:00	243234	77	100	
13:58:00	243622	75	97,4025974	
13:59:00	243768	76	101,3333333	
14:00:00	247505	102	134,2105263	!!!
14:01:00	245440	90	88,23529412	
14:02:00	245606	87	96,66666667	
14:03:00	244296	82	94,25287356	
14:04:00	244467	82	100	
14:05:00	245643	88	107,3170732	!
14:06:00	244674	85	96,59090909	
14:07:00	244468	83	97,64705882	
14:08:00	244526	82	98,79518072	
14:09:00	244007	80	97,56097561	
14:10:00	246297	90	112,5	!!
14:11:00	244924	84	93,33333333	
14:12:00	245254	83	98,80952381	
14:13:00	244791	81	97,59036145	
14:14:00	243609	78	96,2962963	
14:15:00	247079	89	114,1025641	!!
14:16:00	246003	84	94,38202247	
14:17:00	244329	80	95,23809524	
14:18:00	244942	80	100	
14:19:00	245590	81	101,25	
14:20:00	246190	84	103,7037037	
14:21:00	245222	81	96,42857143	
14:22:00	245355	81	100	
14:23:00	245068	80	98,7654321	
14:24:00	245268	81	101,25	
14:25:00	246810	87	107,4074074	!
14:26:00	245919	83	95,40229885	
14:27:00	246245	85	102,4096386	
14:28:00	245657	83	97,64705882	
14:29:00	245678	82	98,79518072	
14:30:00	249359	95	115,8536585	!!
14:31:00	247704	92	96,84210526	
14:32:00	248263	92	100	
14:33:00	247090	89	96,73913043	
14:34:00	247088	87	97,75280899	
14:35:00	247839	93	106,8965517	!
14:36:00	246774	89	95,69892473	
14:37:00	247150	89	100	
14:38:00	246546	87	97,75280899	
14:39:00	245884	85	97,70114943	
14:40:00	247652	92	108,2352941	!
14:41:00	246983	88	95,65217391	
14:42:00	247760	90	102,2727273	
14:43:00	247015	89	98,88888889	
14:44:00	246271	83	93,25842697	
14:45:00	248179	91	109,6385542	!
14:46:00	246523	86	94,50549451	
14:47:00	247832	87	101,1627907	
14:48:00	246916	87	100	
14:49:00	247913	87	100	
14:50:00	249086	92	105,7471264	!
14:51:00	248370	89	96,73913043	
14:52:00	248077	88	98,87640449	
14:53:00	247795	87	98,86363636	
14:54:00	247658	86	98,85057471	
14:55:00	250434	96	111,627907	!!
14:56:00	249434	91	94,79166667	
14:57:00	249677	91	100	
14:58:00	248999	91	100	
14:59:00	249420	92	101,0989011	
15:00:00	251401	108	117,3913043	!!
15:01:00	249992	101	93,51851852	
15:02:00	249756	97	96,03960396	
15:03:00	249181	96	98,96907216	
15:04:00	249741	98	102,0833333	
15:05:00	250433	104	106,122449	!
15:06:00	249617	99	95,19230769	
15:07:00	249986	101	102,020202	
15:08:00	250410	100	99,00990099	
15:09:00	249865	98	98	
15:10:00	251386	109	111,2244898	!!
15:11:00	250568	103	94,49541284	
15:12:00	251049	104	100,9708738	
15:13:00	250770	102	98,07692308	
15:14:00	250989	100	98,03921569	
15:15:00	252727	112	112	!!
15:16:00	251749	106	94,64285714	
15:17:00	251453	103	97,16981132	
15:18:00	251416	103	100	
15:19:00	252061	105	101,9417476	
15:20:00	253220	113	107,6190476	!
15:21:00	252759	111	98,2300885	
15:22:00	252638	107	96,3963964	
15:23:00	252513	108	100,9345794	
15:24:00	252711	106	98,14814815	
15:25:00	253523	114	107,5471698	!
15:26:00	253419	112	98,24561404	
15:27:00	253351	110	98,21428571	
15:28:00	253772	112	101,8181818	
15:29:00	254002	112	100	
15:30:00	256782	147	131,25	!!!
15:31:00	256127	135	91,83673469	
15:32:00	256082	134	99,25925926	
15:33:00	255796	131	97,76119403	
15:34:00	255770	131	100	
15:35:00	256612	139	106,1068702	!
15:36:00	256324	137	98,56115108	
15:37:00	256481	136	99,27007299	
15:38:00	256289	135	99,26470588	
15:39:00	256648	138	102,2222222	
15:40:00	257635	157	113,7681159	!!
15:41:00	257436	154	98,08917197	
15:42:00	257560	156	101,2987013	
15:43:00	257726	157	100,6410256	
15:44:00	257995	164	104,4585987	
15:45:00	259116	204	124,3902439	!!!
15:46:00	258637	186	91,17647059	
15:47:00	258925	187	100,5376344	
15:48:00	259017	190	101,6042781	
15:49:00	259207	200	105,2631579	!
15:50:00	260072	339	169,5	!!!
15:51:00	259802	272	80,2359882	
15:52:00	259823	267	98,16176471	
15:53:00	259877	275	102,9962547	
15:54:00	260121	345	125,4545455	!!!
15:55:00	260210	410	118,8405797	!!
15:56:00	260189	376	91,70731707	
15:57:00	260284	410	109,0425532	!
15:58:00	260312	469	114,3902439	!!
15:59:00	260384	846	180,3837953	!!!
*/
    public class TradesPerMinute
    {
        public static void Start()
        {
            var result = new Dictionary<TimeSpan, int[]>();
            var quoteCount = 0;
            foreach (var oo in Data.Actions.Polygon.PolygonMinuteScan.GetQuotes(new DateTime(2023, 1, 1),
                         new DateTime(2024, 1, 1)))
            {
                quoteCount += oo.Item3.Length;
                foreach (var quote in oo.Item3.Where(a => a.o >= 5.0f && Settings.IsInMarketTime(a.DateTime)))
                {
                    var time = quote.DateTime.TimeOfDay;
                    if (!result.ContainsKey(time))
                    {
                        result.Add(time, new int[2]);
                    }

                    result[time][0]++;
                    result[time][1] += quote.n;
                }
            }

            foreach (var kvp in result.OrderBy(a => a.Key))
                Debug.Print($"{kvp.Key}\t{kvp.Value[0]}\t{kvp.Value[1]/kvp.Value[0]}");

            Debug.Print($"Quote count: {quoteCount:N0}");
        }
    }
}