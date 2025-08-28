
const fs = require("fs");

let tables = `2.2.1	AFW_ActivityAction	22
2.2.2	AFW_ActivityChkpt	22
2.2.3	AFW_ActivityChkptCompany	23
2.2.4	AFW_AddOtherInterest	23
2.2.5	AFW_Agency	24
2.2.6	AFW_AgencyAddr	30
2.2.7	AFW_AgencycheckList	31
2.2.8	AFW_AgencyDirectBillDeferredGLAccount	31
2.2.9	AFW_AgencyNotation	32
2.2.10	AFW_AgencyXRefType	32
2.2.11	AFW_Agent	33
2.2.12	AFW_APTInsurer	33
2.2.13	AFW_AuthLoginTime	34
2.2.14	AFW_BankAccount	34
2.2.15	AFW_Broker	36
2.2.16	AFW_BusinessUnitAccess	38
2.2.17	AFW_BusinessUnitAccessKey	39
2.2.18	AFW_BusinessUnitAllocBreakOut	39
2.2.19	AFW_BusinessUnitAllocDivSplit	41
2.2.20	AFW_BusinessUnitAllocHeader	41
2.2.21	AFW_BusinessUnitHierarchy	42
2.2.22	AFW_BusinessUnitStaging	42
2.2.23	AFW_BusUnitAgencyConfig	43
2.2.24	AFW_BusUnitAssignment	44
2.2.25	AFW_BusUnitBrokerFilter	44
2.2.26	AFW_BusUnitCompanyFilter	45
2.2.27	AFW_BusUnitEmployeeFilter	45
2.2.28	AFW_BusUnitVendorFilter	46
2.2.29	AFW_Company	46
2.2.30	AFW_CompanyAddress	49
2.2.31	AFW_CompanyBillOptions	51
2.2.32	AFW_CompanyPlan	52
2.2.33	AFW_CompSpecQuestHead	52
2.2.34	AFW_CompSpecQuestion	53
2.2.35	AFW_CompSpecQuestRel	53
2.2.36	AFW_CompStateSpecific	54
2.2.37	AFW_CovCodeCat	55
2.2.38	AFW_CoverageCode	55
2.2.39	AFW_CustCertHolder	56
2.2.40	AFW_CustCertHolderGroup	59
2.2.41	AFW_DefaultAgencyCommission	59
2.2.42	AFW_DefaultCommission	60
2.2.43	AFW_DefaultTieredCommission	61
2.2.44	AFW_DefaultTieredCommissionLevel	62
2.2.45	AFW_EmpLicense	62
2.2.46	AFW_Employee	63
2.2.47	AFW_EmsAsyncTaskDetail	67
2.2.48	AFW_EmsAsyncTaskHeader	67
2.2.49	AFW_EquineAge	68
2.2.50	AFW_EquineBreed	68
2.2.51	AFW_EquineCovUse	69
2.2.52	AFW_EquineEligibleCov	69
2.2.53	AFW_EquineOverAge	70
2.2.54	AFW_EquinePlan	70
2.2.55	AFW_EquineRate	70
2.2.56	AFW_EquineSex	71
2.2.57	AFW_EquineStateTax	71
2.2.58	AFW_EquineUse	72
2.2.59	AFW_FaxVendorSetup	72
2.2.60	AFW_FaxUserSetup	73
2.2.61	AFW_FormNo	73
2.2.62	AFW_FormNoCat	74
2.2.63	AFW_GeneralLedgerAccounts	74
2.2.64	AFW_GeneralLedgerBranch	77
2.2.65	AFW_GeneralLedgerBranchDept	77
2.2.66	AFW_GeneralLedgerDepartment	78
2.2.67	AFW_GeneralLedgerDeptGroup	78
2.2.68	AFW_GeneralLedgerDivBranch	79
2.2.69	AFW_GeneralLedgerDivision	79
2.2.70	AFW_GeneralLedgerFiscalYearEnd	80
2.2.71	AFW_GeneralLedgerGroup	83
2.2.72	AFW_GeneralLedgerPeriod	83
2.2.73	AFW_GeneralLedgerSystemAccount	84
2.2.74	AFW_GlobalChangeBusinessUnitDetail	86
2.2.75	AFW_GlobalChangeBusinessUnitHeader	87
2.2.76	AFW_GlobalChangeCollection	88
2.2.77	AFW_GlobalChangeCollectionHeaderMap	89
2.2.78	AFW_ImageAssign	90
2.2.79	AFW_ImageIndex	90
2.2.80	AFW_InvoiceSplitTemplate	91
2.2.81	AFW_LOBSetup	91
2.2.82	AFW_MasterAgent	92
2.2.83	AFW_ONSContact	93
2.2.84	AFW_ONSEntity	94
2.2.85	AFW_ONSRecipient	94
2.2.86	AFW_ONSRecipientEntity	95
2.2.87	AFW_ONSServiceClient	95
2.2.88	AFW_PaymentPlan	96
2.2.89	AFW_PaymentPlanDistribution	96
2.2.90	AFW_PaymentPlanFee	97
2.2.91	AFW_PaymentPlanRounding	97
2.2.92	AFW_PFVIntegration	98
2.2.93	AFW_PlanSetup	98
2.2.94	AFW_PrCode	99
2.2.95	AFW_PrintAssign	100
2.2.96	AFW_ProcListInstance	101
2.2.97	AFW_ProcListTaskInstance	101
2.2.98	AFW_ProfileQuestion	102
2.2.99	AFW_PrString	102
2.2.100	AFW_RefGroup	103
2.2.101	AFW_RelationshipType	104
2.2.102	AFW_Sequence	104
2.2.103	AFW_SigAuth	104
2.2.104	AFW_Underwriter	105
2.2.105	AFW_UserCenterAssign	106
2.2.106	AFW_UserSetting	106
2.2.107	AFW_Vendor	107
2.2.108	AFW_WSUser	110
3	Customer Tables	110
3.1	Entity Diagrams	112
3.2	Tables	113
3.2.1	AFW_CustAddPersonnel	113
3.2.2	AFW_CustContact	113
3.2.3	AFW_CustContactResp	115
3.2.4	AFW_CustLossHist	115
3.2.5	AFW_Customer	116
3.2.6	AFW_CustomerAttribute	124
3.2.7	AFW_CustomerAttributeType	125
3.2.8	AFW_CustomerInternetAccess	125
3.2.9	AFW_CustomerRelationship	126
3.2.10	AFW_CustXRef	127
3.2.11	AFW_Dependent	127
3.2.12	AFW_MergeCustomer	129
3.2.13	AFW_MergeCustomerResult	130
3.2.14	AFW_MergeCustomerResultCount	130
3.2.15	AFW_ProfileAnswer	131
3.2.16	AFW_RecentCustomer	131
3.2.17	AFW_Relationship	131
3.2.18	AFW_XDate	132
4	Policy Tables	133
4.1	History	133
4.1.1	Endorsements to a Policy	134
4.1.2	Back dated Endorsements	135
4.2	Policy vs. Submission	135
4.3	General Tables	136
4.3.1	Overview	136
4.3.2	Entity Diagrams	137
4.3.3	Tables	139
4.3.3.1	AFW_Address	139
4.3.3.2	AFW_BasicPolInfo	140
4.3.3.3	AFW_CommissionTemplate	146
4.3.3.4	AFW_CommissionTemplatePersonnel	147
4.3.3.5	AFW_ExternalEntity	148
4.3.3.6	AFW_LineOfBusiness	148
4.3.3.7	AFW_PhoneNumber	150
4.3.3.8	AFW_PolContact	150
4.3.3.9	AFW_PolicyAttribute	153
4.3.3.10	AFW_PolicyAttributeType	153
4.3.3.11	AFW_PolicyChkLstDetail	153
4.3.3.12	AFW_PolicyChkLstHeader	155
4.3.3.13	AFW_PolicyPersonnel	156
4.3.3.14	AFW_PolicyPersonnelPeriods	157
4.3.3.15	AFW_PolicySubCustomer	158
4.3.3.16	AFW_PolicyTranPremium	158
4.3.3.17	AFW_PolicyTransaction	162
4.3.3.18	AFW_Submission	163
4.3.3.19	AFW_SubmissionGroup	163
4.4	Policy History Tables	164
4.4.1	Overview	164
4.4.2	Entity Diagrams	165
4.4.3	Tables	191
4.4.3.1	AFW_125NatureOfBus	191
4.4.3.2	AFW_125UWSignature	191
4.4.3.3	AFW_126SClaimsMade	193
4.4.3.4	AFW_126SContractor	193
4.4.3.5	AFW_126SCoverage	194
4.4.3.6	AFW_126SHazard	195
4.4.3.7	AFW_126SPCO	196
4.4.3.8	AFW_126SPCOQuestion	197
4.4.3.9	AFW_127Coverage1	198
4.4.3.10	AFW_127Driver	203
4.4.3.11	AFW_127UnderWriting	204
4.4.3.12	AFW_127Vehicle	206
4.4.3.13	AFW_128BusinessInfo	208
4.4.3.14	AFW_128CovAutoSymbol	209
4.4.3.15	AFW_128DealersDamage	212
4.4.3.16	AFW_128GarageOperation	213
4.4.3.17	AFW_128GarKeepers	214
4.4.3.18	AFW_128StorageInfo	215
4.4.3.19	AFW_130InclExcl	216
4.4.3.20	AFW_130Policy	217
4.4.3.21	AFW_130Rating	219
4.4.3.22	AFW_130Submit	220
4.4.3.23	AFW_131SAddExposure	221
4.4.3.24	AFW_131SCoverage	224
4.4.3.25	AFW_131SCus	226
4.4.3.26	AFW_131SGLInfo	227
4.4.3.27	AFW_131SLocation	229
4.4.3.28	AFW_131SPolicy	230
4.4.3.29	AFW_131SUnderlyingPolicy	230
4.4.3.30	AFW_131SVehicle	232
4.4.3.31	AFW_132Authority	234
4.4.3.32	AFW_132Commodities	234
4.4.3.33	AFW_132Coverage	235
4.4.3.34	AFW_132Equipment	244
4.4.3.35	AFW_132Receipts	245
4.4.3.36	AFW_132Regulation	246
4.4.3.37	AFW_132Terminal	248
4.4.3.38	AFW_132TrailerInterchange	248
4.4.3.39	AFW_140PremiseInfo	249
4.4.3.40	AFW_140SubOfIns	252
4.4.3.41	AFW_140ValueRpt	254
4.4.3.42	AFW_141Classification	255
4.4.3.43	AFW_141Classification2	255
4.4.3.44	AFW_141Control	256
4.4.3.45	AFW_141DepositoryInfo	259
4.4.3.46	AFW_141Employee	260
4.4.3.47	AFW_141ERISAInfo	261
4.4.3.48	AFW_141ERISAPlan	262
4.4.3.49	AFW_141GeneralInfo	262
4.4.3.50	AFW_141Messenger	264
4.4.3.51	AFW_141MessengerProtection	265
4.4.3.52	AFW_141Money	266
4.4.3.53	AFW_141Property	267
4.4.3.54	AFW_141Rating	267
4.4.3.55	AFW_141SafeProtection	268
4.4.3.56	AFW_141Vault	270
4.4.3.57	AFW_143FOBGenInfoRmk	271
4.4.3.58	AFW_143InterestType	272
4.4.3.59	AFW_143MTCCommodity	273
4.4.3.60	AFW_143MTCLegalLiability	274
4.4.3.61	AFW_143MTCOperation	275
4.4.3.62	AFW_143MTCStateFiling	276
4.4.3.63	AFW_143Terminal	277
4.4.3.64	AFW_143TransConveyance	278
4.4.3.65	AFW_143TransOperation	279
4.4.3.66	AFW_144Glass	280
4.4.3.67	AFW_144Sign	281
4.4.3.68	AFW_145Account	282
4.4.3.69	AFW_145BldgConstruction	283
4.4.3.70	AFW_145Location	285
4.4.3.71	AFW_145Papers	286
4.4.3.72	AFW_145Receivables	287
4.4.3.73	AFW_145RecordLocation	287
4.4.3.74	AFW_145SafeProtection	289
4.4.3.75	AFW_145Vault	290
4.4.3.76	AFW_146EquipFloater	292
4.4.3.77	AFW_146EquipSummary	292
4.4.3.78	AFW_146Locations	293
4.4.3.79	AFW_146SchedEquip	294
4.4.3.80	AFW_146Storage	295
4.4.3.81	AFW_146UnschedEquip	296
4.4.3.82	AFW_147BuilderOperation	297
4.4.3.83	AFW_147JobValue	298
4.4.3.84	AFW_147RigTransSecurity	299
4.4.3.85	AFW_147SpecificJob	300
4.4.3.86	AFW_148MediaInfo	301
4.4.3.87	AFW_148Schedule	303
4.4.3.88	AFW_148Underwriting	304
4.4.3.89	AFW_AnnualPol	306
4.4.3.90	AFW_Applicant	307
4.4.3.91	AFW_ApplicantPhoneMap	311
4.4.3.92	AFW_Attachment	311
4.4.3.93	AFW_Boat	312
4.4.3.94	AFW_BoatEngine	314
4.4.3.95	AFW_BoatEquipment	315
4.4.3.96	AFW_BoatExperience	316
4.4.3.97	AFW_BoatOperator	317
4.4.3.98	AFW_BoatSummary	318
4.4.3.99	AFW_BoatTrailer	319
4.4.3.100	AFW_Building	320
4.4.3.101	AFW_CBuilding	320
4.4.3.102	AFW_CForm	322
4.4.3.103	AFW_CLocation	323
4.4.3.104	AFW_CNamedInsured	324
4.4.3.105	AFW_CNamedInsuredPhoneMap	326
4.4.3.106	AFW_CoInsured	326
4.4.3.107	AFW_CoInsuredPhoneMap	328
4.4.3.108	AFW_CommAddOtherInt	328
4.4.3.109	AFW_CompSpecAnswer	331
4.4.3.110	AFW_Conviction	331
4.4.3.111	AFW_Coverage	332
4.4.3.112	AFW_CoverageHome	333
4.4.3.113	AFW_CovOption	335
4.4.3.114	AFW_CPrem	335
4.4.3.115	AFW_CPremTotal	340
4.4.3.116	AFW_CustBenefitInfo	341
4.4.3.117	AFW_DriveOtherCar	341
4.4.3.118	AFW_Driver	342
4.4.3.119	AFW_Employer	344
4.4.3.120	AFW_ExBoatCoverage	345
4.4.3.121	AFW_Factor	346
4.4.3.122	AFW_FarmCategory	347
4.4.3.123	AFW_FarmExclProp	348
4.4.3.124	AFW_FarmGL	350
4.4.3.125	AFW_FarmItem	350
4.4.3.126	AFW_FarmPIUW	351
4.4.3.127	AFW_FarmPremiseInfo	353
4.4.3.128	AFW_FarmPropUW	354
4.4.3.129	AFW_FarmRanch	356
4.4.3.130	AFW_FarmSubOfIns	357
4.4.3.131	AFW_FarmUW	358
4.4.3.132	AFW_Filing	359
4.4.3.133	AFW_FloodLocation	360
4.4.3.134	AFW_FloodRating	362
4.4.3.135	AFW_FloodSectionOne	364
4.4.3.136	AFW_FloodSectionTwo	366
4.4.3.137	AFW_FloodTotal	368
4.4.3.138	AFW_Form	369
4.4.3.139	AFW_FormType	370
4.4.3.140	AFW_Garage	371
4.4.3.141	AFW_HealthCoverage	372
4.4.3.142	AFW_HealthMember	373
4.4.3.143	AFW_HealthPrem	374
4.4.3.144	AFW_HiredBorrowed	375
4.4.3.145	AFW_HomeFeature	376
4.4.3.146	AFW_HomeRating	377
4.4.3.147	AFW_HomeReplacement	382
4.4.3.148	AFW_Horse	384
4.4.3.149	AFW_HorsePlanLOB	385
4.4.3.150	AFW_LifeBeneficiary	385
4.4.3.151	AFW_LifeCoverage	386
4.4.3.152	AFW_LifeOtherInsurance	388
4.4.3.153	AFW_LifeOwner	389
4.4.3.154	AFW_Location	390
4.4.3.155	AFW_LossHistory	392
4.4.3.156	AFW_MobileHome	393
4.4.3.157	AFW_Name	394
4.4.3.158	AFW_NonOwned	395
4.4.3.159	AFW_PersonalUmbrella	396
4.4.3.160	AFW_PersUmbrellaRating	397
4.4.3.161	AFW_Physician	398
4.4.3.162	AFW_PolContactPhoneMap	400
4.4.3.163	AFW_PolUmbrella	400
4.4.3.164	AFW_PProducer	401
4.4.3.165	AFW_PrevAddr	402
4.4.3.166	AFW_PriorCarrier	403
4.4.3.167	AFW_QUEIDQuestionAdditionalInfo	406
4.4.3.168	AFW_QUEIDQuestions	406
4.4.3.169	AFW_RateDate	407
4.4.3.170	AFW_Record	407
4.4.3.171	AFW_Remark	408
4.4.3.172	AFW_ServiceAgreement	409
4.4.3.173	AFW_ServiceAgreementPolicies	410
4.4.3.174	AFW_SnowMobile	410
4.4.3.175	AFW_SpecBuilding	411
4.4.3.176	AFW_SpecLocation	412
4.4.3.177	AFW_SpecRatingAnswer	413
4.4.3.178	AFW_SpecRisk	414
4.4.3.179	AFW_SpecRiskAnswer	415
4.4.3.180	AFW_SpecUnderwritingAnswer	416
4.4.3.181	AFW_SPPAddInfo	416
4.4.3.182	AFW_SPPItem	417
4.4.3.183	AFW_SPPSummary	418
4.4.3.184	AFW_Submit	420
4.4.3.185	AFW_UmbrellaPrem	420
4.4.3.186	AFW_UnderWriting	421
4.4.3.187	AFW_UnsupportedData	425
4.4.3.188	AFW_Usage	426
4.4.3.189	AFW_Vehicle	427
4.4.3.190	AFW_Watercraft	430
5	eForms Tables	431
5.1	Overview	431
5.2	Entity Diagrams	433
5.3	Tables	434
5.3.1	AFW_Binder	434
5.3.2	AFW_BinderAOI	435
5.3.3	AFW_CertHolderInfo	436
5.3.4	AFW_CertLiabProp	437
5.3.5	AFW_Claim	439
5.3.6	AFW_ClaimContact	441
5.3.7	AFW_ClaimInjured	442
5.3.8	AFW_ClaimPayment	443
5.3.9	AFW_ClaimPropDamage	444
5.3.10	AFW_ClaimRemark	445
5.3.11	AFW_ClaimRiskInfo	446
5.3.12	AFW_EvidenceOfProp	447
5.3.13	AFW_EvidenceOfPropAOI	447
5.3.14	AFW_GLInfo	448
5.3.15	AFW_Injury	449
5.3.16	AFW_LossDriver	450
5.3.17	AFW_LossGenLiab	450
5.3.18	AFW_LossVehicle	451
5.3.19	AFW_PolicyChange	452
5.3.20	AFW_PolicyChangeAOI	453
6	Accounting Tables	454
6.1.1	Setup	454
6.1.2	General Ledger	455
6.1.3	Invoice/Billing	455
6.1.4	Journal Entry	456
6.1.5	Cash Disbursement	456
6.1.6	Cash Receipt	456
6.1.7	Payable	456
6.1.8	Direct Bill Statement	457
6.1.9	Direct Bill Reconciliation	457
6.1.10	Bank Reconciliation	457
6.1.11	Budget	457
6.2	Entity Diagrams	459
6.3	Tables	460
6.3.1	AFW_BalanceJournalEntryCollection	460
6.3.2	AFW_BankPaymentType	460
6.3.3	AFW_BankRecCleared	461
6.3.4	AFW_BankRecStatement	462
6.3.5	AFW_BankStatementDetail	463
6.3.6	AFW_BankStatementHeader	463
6.3.7	AFW_BillingHeader	464
6.3.8	AFW_CashDisbursment	466
6.3.9	AFW_CashReceiptDeposit	468
6.3.10	AFW_CashReceiptDetail	469
6.3.11	AFW_CashReceiptHeader	470
6.3.12	AFW_CommissionIE	471
6.3.13	AFW_DirectBillDownloadDetail	473
6.3.14	AFW_DirectBillDownloadHeader	475
6.3.15	AFW_DirectBillEntryCommission	475
6.3.16	AFW_DirectBillEntryDetail	477
6.3.17	AFW_DirectBillEntryHeader	479
6.3.18	AFW_DirectBillStmtAutoRec	480
6.3.19	AFW_DirectBillStmtDetail	481
6.3.20	AFW_DirectBillStmtHeader	484
6.3.21	AFW_FinancialImportDetail	488
6.3.22	AFW_FinancialImportGLAccount	489
6.3.23	AFW_FinancialImportHeader	491
6.3.24	AFW_GeneralJournal	491
6.3.25	AFW_GeneralJournalRelation	493
6.3.26	AFW_GeneralLedgerBudgetDetail	494
6.3.27	AFW_GeneralLedgerBudgetDivDept	494
6.3.28	AFW_GeneralLedgerBudgetHeader	495
6.3.29	AFW_GeneralLedgerInactiveAccountsForDiv	495
6.3.30	AFW_GeneralLedgerTransaction	496
6.3.31	AFW_Invoice	498
6.3.32	AFW_InvoiceBatch	501
6.3.33	AFW_InvoiceBillingEvent	502
6.3.34	AFW_InvoiceBillTo	503
6.3.35	AFW_InvoiceCollection	505
6.3.36	AFW_InvoiceCommission	505
6.3.37	AFW_InvoiceCompanyPremium	507
6.3.38	AFW_InvoiceCustPremium	508
6.3.39	AFW_InvoiceMessage	509
6.3.40	AFW_InvoiceTransaction	510
6.3.41	AFW_JournalMemo	513
6.3.42	AFW_OnAccountInvoice	513
6.3.43	AFW_Payee	515
6.3.44	AFW_PayStatementDetail	515
6.3.45	AFW_PayStatementHeader	518
6.3.46	AFW_PendingCheckGLT	521
6.3.47	AFW_PolicyBillingAddress	524
6.3.48	AFW_PolicyCompanyPremium	526
6.3.49	AFW_PolicyCustPremium	528
6.3.50	AFW_PrintedCheck	529
6.3.51	AFW_PrintedReceipt	530
6.3.52	AFW_ProcessMessage	531
6.3.53	AFW_RecurringItem	532
6.3.54	AFW_StatementHistory	534
6.3.55	AFW_TrustTransferStatementDetail	536
6.3.56	AFW_TrustTransferStatementHeader	538
6.3.57	AFW_VendorInvoice	539
6.3.58	AFW_VendorInvoiceTemplate	540
6.3.59	AFW_VendorInvoiceTransaction	541
6.3.60	AFW_VendorPayment	542
6.3.61	AFW_VoidedCheck	543
7	Marketing Tables	543
7.1	Overview	543
7.2	Entity Diagrams	545
7.3	Tables	546
7.3.1	AFW_Campaign	546
7.3.2	AFW_CampaignAction	547
7.3.3	AFW_CampaignParticipant	548
7.3.4	AFW_CampaignProspectList	550
7.3.5	AFW_CpnProspListMember	550
7.3.6	AFW_LeadList	551
7.3.7	AFW_LeadListDef	552
7.3.8	AFW_LeadListDefDetail	552
7.3.9	AFW_LeadListDefElement	553
7.3.10	AFW_LeadListIntCCnt	553
7.3.11	AFW_LeadListIntCCntR	554
7.3.12	AFW_LeadListIntCust	554
7.3.13	AFW_LeadListIntProfA	555
7.3.14	AFW_LeadListIntXDat	555
7.3.15	AFW_TargetListBPol	556
7.3.16	AFW_TargetListCustomer	557
7.3.17	AFW_TargetListLOB	560
7.3.18	AFW_TargetListLOBCoverage	560
7.3.19	AFW_TargetListProfile	562
7.3.20	AFW_TargetListXDate	562
8	Download Tables	564
8.1	Overview	564
8.2	Entity Diagrams	565
8.3	Tables	566
8.3.1	AFW_A3Company	566
8.3.2	AFW_A3Element	566
8.3.3	AFW_A3Group	567
8.3.4	AFW_A3Relationship	567
8.3.5	AFW_DownloadAddress	568
8.3.6	AFW_DownLoadEmail	569
8.3.7	AFW_DownLoadEmailHead	569
8.3.8	AFW_DownloadGroup	570
8.3.9	AFW_DownLoadMessage	571
8.3.10	AFW_DownLoadReport	572
8.3.11	AFW_DownLoadTran	573
8.3.12	AFW_DownLoadTranSeq	575
9	Misc. Tables	576
9.1	Entity Diagrams	576
9.2	Tables	576
9.2.1	AFW_AppAccessToAgency	576
9.2.2	AFW_AppUpdateRules	577
9.2.3	AFW_AuditTrailInfo	577
9.2.4	AFW_BankStatementHeaderDetailMap	578
9.2.5	AFW_CollectionLetterDetail	578
9.2.6	AFW_CollectionLetterHeader	579
9.2.7	AFW_DataPorterFiles	580
9.2.8	AFW_DocAttachment	581
9.2.9	AFW_DocClassificationSetup	584
9.2.10	AFW_DocRelation	585
9.2.11	AFW_DocRouting	585
9.2.12	AFW_EmsAsyncTaskStatus	586
9.2.13	AFW_ESignature	586
9.2.14	AFW_EventLog	587
9.2.15	AFW_ExternalReference	588
9.2.16	AFW_Image	588
9.2.17	AFW_Notes	588
9.2.18	AFW_ONSActivityAction	590
9.2.19	AFW_ONSMessageToProcess	591
9.2.20	AFW_PurgeDetail	592
9.2.21	AFW_PurgeDetailGroup	593
9.2.22	AFW_PurgeHeader	593
9.2.23	AFW_RenewalListCC	594
9.2.24	AFW_RenewalListDetail	594
9.2.25	AFW_RenewalListHeader	595
9.2.26	AFW_RenewalListPolicy	596
9.2.27	AFW_Suspense	596
9.2.28	AFW_SuspenseCC	599
9.2.29	AFW_Transaction	599
9.2.30	AFW_TransactionAdditionalComments	603
9.2.31	AFW_VimCacheRepository`;

let tableRows = tables.split("\n");
let tableNames = []

let customerTables = []
let policyTables = []
let activityTables = []
let transactionTables = []
let firstPassTables = []

tableRows.forEach(tableRow => {
    if (tableRow.includes("AFW")) {
        let tableName = "AFW" + tableRow.split("AFW")[1].split("\t").join(" ").split(" ")[0];
        if (!tableNames.includes(tableName)){
            tableNames.push(tableName);
        }
    }
})

tableNames.forEach(tableName => {
    if (tableName.toLowerCase().includes("cust")) {
        customerTables.push(tableName)
        if (!firstPassTables.includes(tableName)){
            firstPassTables.push(tableName)
        }
    }
    if (tableName.toLowerCase().includes("activity")) {
        activityTables.push(tableName)
        if (!firstPassTables.includes(tableName)){
            firstPassTables.push(tableName)
        }
    }
    if (tableName.toLowerCase().includes("pol")) {
        policyTables.push(tableName)
        if (!firstPassTables.includes(tableName)){
            firstPassTables.push(tableName)
        }
    }
    if (tableName.toLowerCase().includes("trans")) {
        transactionTables.push(tableName)
        if (!firstPassTables.includes(tableName)){
            firstPassTables.push(tableName)
        }
    }
})

console.log(tableNames)

writeResult(JSON.stringify(tableNames), "./tableNames.json")
writeResult(JSON.stringify(activityTables), "./activityTables.json")
writeResult(JSON.stringify(customerTables), "./customerTables.json")
writeResult(JSON.stringify(transactionTables), "./transactionTables.json")
writeResult(JSON.stringify(policyTables), "./policyTables.json")
writeResult(JSON.stringify(firstPassTables), "./firstPassTables.json")



function writeResult(content, fileName) {
    fs.writeFile(fileName, content, err => {
        if (err) {
            console.error(err);
        }
    });
}
