package opinionfinder;

import opin.config.Config;
import opin.entity.Corpus;
import opin.featurefinder.ClueFind;
import opin.logic.AnnotationHandler;
import opin.preprocessor.PreProcess;
import opin.supervised.SentenceSubjectivityClassifier;

public class step1 {

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		String[] step2Process = { "test.doclist" };

		for (int i = 0; i < step2Process.length; i++) {
			String[] opinion = { step2Process[i], "-d" };

			/*
			 * Reading command line arguments
			 */
			Config conf = new Config();
			if (!conf.parseCommandLineOptions(opinion)) {
				System.exit(-1);
			}
			/*
			 * Creating corpus object to process
			 */
			Corpus corpus = new Corpus(conf);

			/*
			 * Pre-processing the corpus
			 */
			if (conf.isRunPreprocessor()) {
				PreProcess preprocessor = new PreProcess(conf);
				preprocessor.process(corpus);
			}

			/*
			 * Finding lexicon clues in the corpus
			 */

			if (conf.isRunClueFinder()) {
				ClueFind clueFinder = new ClueFind(conf);
				clueFinder.process(corpus);
			}

			/*
			 * Prepare data for classification
			 */

			AnnotationHandler annHandler = new AnnotationHandler(conf);
			if (conf.isRunRulebasedClassifier() || conf.isRunSubjClassifier() || conf.isRunPolarityClassifier()) {
				annHandler.buildSentencesFromGateDefault(corpus);
			}

			/*
			 * Applying rule-based classifier to the corpus
			 */
			/*
			 * if(conf.isRunRulebasedClassifier()){
			 * annHandler.readInRequiredAnnotationsForRuleBased(corpus);
			 * RuleBasedClassifier rulebased = new RuleBasedClassifier();
			 * rulebased.process(corpus); }
			 */
			/*
			 * Applying subjectivity classifier to the corpus
			 */

			if (conf.isRunSubjClassifier()) {
				annHandler.readInRequiredAnnotationsForSubjClassifier(corpus);
				SentenceSubjectivityClassifier subjClassifier = new SentenceSubjectivityClassifier(conf);
				subjClassifier.process(corpus);
			}

			/*
			 * Applying polarity classifier to the corpus
			 */
			/*
			 * if(conf.isRunPolarityClassifier()){
			 * annHandler.readInRequiredAnnotationsForPolarityClassifier(corpus)
			 * ; ExpressionPolarityClassifier polarityClassifier = new
			 * ExpressionPolarityClassifier(conf);
			 * polarityClassifier.process(corpus); }
			 */
			/*
			 * Creating SGML output
			 */
			/*
			 * if(conf.isRunSGMLOutput()){ SGMLOutput output = new
			 * SGMLOutput(conf.isRunRulebasedClassifier(),
			 * conf.isRunSubjClassifier(), conf.isRunPolarityClassifier());
			 * output.process(corpus); }
			 */

		}
		System.out.println("FINISH!!");

	}

}
